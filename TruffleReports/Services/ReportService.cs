using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TruffleReports.Contracts;
using TruffleReports.Entities;

namespace TruffleReports.Services
{
    /// <summary>
    /// An default implementation of <see cref="IReportService"/> storing results in a Mongo collection.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IEnumerable<IReportProvider> providers;
        private readonly MongoCollection<ReportGenerationSummary> summaryCollection;
        private readonly MongoCollection<Hit> hitCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="defaultDatabase">The default database.</param>
        /// <param name="providers">The providers.</param>
        public ReportService(IEnumerable<IReportProvider> providers, string connectionString, string defaultDatabase = "local")
        {
            this.providers = providers;
            var client = new MongoClient(connectionString);
            var db = client.GetServer().GetDatabase(defaultDatabase);
            summaryCollection = db.GetCollection<ReportGenerationSummary>(Consts.SUMMARY_COLLECTION);
            hitCollection = db.GetCollection<Hit>(Consts.HIT_COLLECTION);
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
            var tasks = providers.AsParallel().Select(async provider => await provider.Generate(hits)).ToArray();
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
    }
}