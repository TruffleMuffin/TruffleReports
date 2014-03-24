using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TruffleReports.Contracts;
using TruffleReports.Helpers;

namespace TruffleReports.Providers.Activity
{
    /// <summary>
    /// Implements a <see cref="IReportProvider"/> that will generate a report about currently logged in users.
    /// </summary>
    public class LoggedInProvider : IReportProvider
    {
        private const string LOGGED_IN_REPORT_COLLECTION = "logged_in";

        private readonly MongoCollection<LoggedInReport> collection;
        private readonly string logOutUrl;

        /// <summary>
        /// Initializes the <see cref="LoggedInProvider"/> class.
        /// </summary>
        static LoggedInProvider()
        {
            // Always register this class correctly.
            if (BsonClassMap.IsClassMapRegistered(typeof(LoggedInReport)) == false)
            {
                BsonClassMap.RegisterClassMap<LoggedInReport>(cm =>
                {
                    cm.AutoMap();
                    cm.IdMemberMap.SetIdGenerator(GuidGenerator.Instance);
                });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedInProvider" /> class.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="logOutUrl">The log out URL.</param>
        public LoggedInProvider(RepositoryHelper helper, string logOutUrl = "/Home/Logout")
        {
            this.collection = helper.Database.GetCollection<LoggedInReport>(LOGGED_IN_REPORT_COLLECTION);
            EnsureIndexes(this.collection);

            this.logOutUrl = logOutUrl;
        }

        /// <summary>
        /// Generates a report
        /// </summary>
        /// <param name="hits">The hits.</param>
        /// <returns>
        /// A <see cref="TruffleReports.Contracts.ReportGenerationResult" /> regarding this instances running.
        /// </returns>
        public Task<ReportGenerationResult> Generate(IEnumerable<Hit> hits)
        {
            var result = new ReportGenerationResult { Provider = this.GetType().FullName };

            // Find all the distinct Identities in Hits - These are definitely currently logged in
            var currentHitsLoggedIn = hits.Select(a => a.Identity).Distinct().ToArray();

            // Find all the Hits that are to the Log Out Url - These are definitely not logged in
            var logOutHits = hits.Where(a => a.Path.Equals(logOutUrl, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            // Addendum - If there is another login after the log out they are still logged in.
            var loggedOutUsers = new List<string>();

            // Find the previous logged in report, use it to scape all the information and merge
            Parallel.ForEach(logOutHits, hit =>
                {
                    if (hits.Any(a =>
                                a.Identity == hit.Identity &&
                                a.Logged > hit.Logged &&
                                a.Path.Equals(logOutUrl, StringComparison.InvariantCultureIgnoreCase) == false) == false)
                    {
                        loggedOutUsers.Add(hit.Identity);
                    }
                });

            // Expand the already collected data and start finalisation process
            var loggedInUsers = currentHitsLoggedIn
                .Where(a => string.IsNullOrWhiteSpace(a) == false)
                .Where(a => loggedOutUsers.Contains(a) == false)
                .Distinct()
                .Select(a => new LoggedInUser { Identity = a })
                .ToList();

            Parallel.ForEach(loggedInUsers, u =>
            {
                var h = hits.Where(b => b.Identity == u.Identity).OrderBy(b => b.Logged).ToArray();
                u.FirstHit = h.FirstOrDefault().Logged;
                u.LastHit = h.LastOrDefault().Logged;
                u.TotalHits = h.Length;
            });

            // Find all the users in the new report that have had no activity for the defined period of time before forced logout.
            var previousReport = collection.FindAll().SetSortOrder(SortBy<LoggedInReport>.Descending(a => a.Generated)).SetLimit(1).FirstOrDefault();
            if (previousReport != null)
            {
                Parallel.ForEach(previousReport.Users, u =>
                    {
                        // if they havnt logged in in a while remove them from final list
                        if (currentHitsLoggedIn.Contains(u.Identity) == false && u.LastHit.AddMinutes(10) < DateTime.Now)
                        {
                            loggedInUsers = loggedInUsers.Where(a => a.Identity == u.Identity).ToList();
                        }
                        else if (loggedOutUsers.Contains(u.Identity) == false)
                        {
                            // Otherwise update the information / add to it
                            var item = loggedInUsers.FirstOrDefault(a => a.Identity == u.Identity);
                            if (item == null)
                            {
                                loggedInUsers.Add(u);
                            }
                            else
                            {
                                item.FirstHit = u.FirstHit;
                                item.LastHit = u.LastHit > item.LastHit ? u.LastHit : item.LastHit;
                                item.TotalHits += u.TotalHits;
                            }
                        }
                    });
            }

            // Finalise data
            Parallel.ForEach(loggedInUsers, u => u.AveragePerHit = TimeSpan.FromSeconds((u.LastHit - u.FirstHit).TotalSeconds / u.TotalHits));

            // Generate report
            var report = new LoggedInReport
            {
                Generated = DateTime.Now,
                Total = loggedInUsers.Count,
                Users = loggedInUsers.ToArray()
            };
            collection.Insert(report);

            // Update report
            result.ReportResult = ReportResult.Success.ToString();

            return Task.FromResult(result);
        }

        /// <summary>
        /// Ensures the default indexes are applied to the collection.
        /// </summary>
        /// <param name="reportCollection">The report collection.</param>
        private static void EnsureIndexes(MongoCollection<LoggedInReport> reportCollection)
        {
            var loggedIndex = new IndexKeysBuilder<LoggedInReport>();
            loggedIndex.Ascending(a => a.Generated);
            reportCollection.EnsureIndex(loggedIndex);
        }
    }
}
