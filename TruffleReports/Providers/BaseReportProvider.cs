using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using TruffleReports.Contracts;

namespace TruffleReports.Providers
{
    /// <summary>
    /// A base report provider that will provide access to the <see cref="Hit"/> collection.
    /// </summary>
    public abstract class BaseReportProvider : IReportProvider
    {
        protected readonly MongoCollection<Hit> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseReportProvider" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="defaultDatabase">The default database.</param>
        protected BaseReportProvider(string connectionString, string defaultDatabase = "local")
        {
            var client = new MongoClient(connectionString);
            var db = client.GetServer().GetDatabase(defaultDatabase);
            collection = db.GetCollection<Hit>(Consts.HIT_COLLECTION);
        }

        /// <summary>
        /// Generates a report
        /// </summary>
        /// <param name="startWindow">The start of the generation window.</param>
        /// <param name="endWindow">The end of the generation window.</param>
        /// <returns>
        /// A <see cref="TruffleReports.Contracts.ReportGenerationResult" /> regarding this instances running.
        /// </returns>
        public abstract Task<ReportGenerationResult> Generate(DateTime startWindow, DateTime endWindow);
    }
}
