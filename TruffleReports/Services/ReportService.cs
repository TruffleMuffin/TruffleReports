using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MongoDB.Driver;
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
        private readonly MongoCollection<ReportGenerationSummary> collection;

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
            collection = db.GetCollection<ReportGenerationSummary>(Consts.SUMMARY_COLLECTION);
        }

        /// <summary>
        /// Generates reports, providing the startDate to scope the generation.
        /// </summary>
        /// <param name="startWindow">The start of the generation window.</param>
        /// <param name="endWindow">The end of the generation window.</param>
        public void Generate(DateTime startWindow, DateTime endWindow)
        {
            var tasks = new List<Task<ReportGenerationResult>>();

            var startDate = DateTime.Now;
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (var reportProvider in providers)
            {
                tasks.Add(reportProvider.Generate(startWindow, endWindow));
            }

            Task.WaitAll(tasks.ToArray());

            stopWatch.Stop();

            var summary = new ReportGenerationSummary { RunAt = startDate, Duration = new TimeSpan(stopWatch.ElapsedTicks) };

            foreach (var task in tasks)
            {
                summary.Results.Add(task.Result);
            }

            collection.Insert(summary);
        }
    }
}