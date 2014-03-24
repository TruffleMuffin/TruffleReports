using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TruffleReports.Contracts;
using TruffleReports.Entities;
using TruffleReports.Helpers;

namespace TruffleReports.Services
{
    /// <summary>
    /// An default implementation of <see cref="ReportService"/> storing results in a Mongo collection.
    /// </summary>
    public class ReportService
    {
        private readonly IDictionary<string, IReportProvider> providers;
        private readonly MongoCollection<ReportGenerationSummary> summaryCollection;
        private readonly MongoCollection<Hit> hitCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService" /> class.
        /// </summary>
        /// <param name="providers">The providers.</param>
        /// <param name="helper">The helper.</param>
        public ReportService(IEnumerable<IReportProvider> providers, RepositoryHelper helper)
        {
            if (providers.Any(a => providers.Count(b => b.Name == a.Name) > 1))
            {
                throw new ArgumentException("Provider names must be unique. The following is duplicated: " + providers.First(a => providers.Count(b => b.Name == a.Name) > 1).Name);
            }

            this.providers = providers.ToDictionary(a => a.Name, a => a);

            summaryCollection = helper.Database.GetCollection<ReportGenerationSummary>(Consts.SUMMARY_COLLECTION);
            hitCollection = helper.Database.GetCollection<Hit>(Consts.HIT_COLLECTION);
        }

        /// <summary>
        /// Generates reports, providing the startDate to scope the generation.
        /// </summary>
        /// <param name="startWindow">The start of the generation window.</param>
        /// <param name="endWindow">The end of the generation window.</param>
        public void Generate(DateTime startWindow, DateTime endWindow)
        {
            var startDate = DateTime.Now;
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var hits = hitCollection.Find(Query.And(Query<Hit>.GTE(a => a.Logged, startWindow), Query<Hit>.LTE(a => a.Logged, endWindow))).ToArray();
            var tasks = providers
                .Values
                .AsParallel()
                .Select(async provider =>
                    {
                        try
                        {
                            return await provider.Generate(hits);
                        }
                        catch (Exception e)
                        {
                            return new ReportGenerationResult
                                {
                                    Provider = provider.GetType().FullName,
                                    ReportResult = ReportResult.UnknownFailure.ToString(),
                                    Messages = new List<string> { e.Message, e.StackTrace }
                                };
                        }
                    })
                .ToArray();

            Task.WaitAll(tasks);

            stopWatch.Stop();

            var summary = new ReportGenerationSummary
                {
                    RunAt = startDate,
                    Duration = new TimeSpan(stopWatch.ElapsedTicks),
                    Results = tasks.Select(a => a.Result).ToArray()
                };

            summaryCollection.Insert(summary);
        }

        /// <summary>
        /// Gets a report of the specified type
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="host">The host.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>
        /// A report.
        /// </returns>
        public Task<object> Get(string name, string host, IEnumerable<KeyValuePair<string, string>> queryString)
        {
            return providers.ContainsKey(name) ? providers[name].Get(host, queryString) : null;
        }
    }
}