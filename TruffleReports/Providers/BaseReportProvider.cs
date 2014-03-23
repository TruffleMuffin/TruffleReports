using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruffleReports.Contracts;

namespace TruffleReports.Providers
{
    /// <summary>
    /// A base report provider that will provide access to the <see cref="Hit"/> collection.
    /// </summary>
    public abstract class BaseReportProvider : IReportProvider
    {
        /// <summary>
        /// The Mongo Database
        /// </summary>
        protected readonly MongoDatabase db;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseReportProvider" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="defaultDatabase">The default database.</param>
        protected BaseReportProvider(string connectionString, string defaultDatabase = "local")
        {
            var client = new MongoClient(connectionString);
            db = client.GetServer().GetDatabase(defaultDatabase);
        }

        /// <summary>
        /// Generates a report
        /// </summary>
        /// <param name="hits">The hits.</param>
        /// <returns>
        /// A <see cref="TruffleReports.Contracts.ReportGenerationResult" /> regarding this instances running.
        /// </returns>
        public abstract Task<ReportGenerationResult> Generate(IEnumerable<Hit> hits);
    }
}
