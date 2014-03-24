using MongoDB.Driver;

namespace TruffleReports.Helpers
{
    /// <summary>
    /// A helper class for accessing the reporting database and its known collections.
    /// </summary>
    public sealed class RepositoryHelper
    {
        /// <summary>
        /// Gets the Mongo Database.
        /// </summary>
        public MongoDatabase Database { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryHelper"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RepositoryHelper(string connectionString)
        {
            var client = new MongoClient(connectionString);
            Database = client.GetServer().GetDatabase(Consts.REPORTS_DATABASE);
        }
    }
}