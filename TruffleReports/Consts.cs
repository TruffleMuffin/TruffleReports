
namespace TruffleReports
{
    /// <summary>
    /// Helper constants for TruffleReports
    /// </summary>
    internal static class Consts
    {
        /// <summary>
        /// The name of the Mongo collection where <see cref="TruffleReports.Contracts.Hit"/>s are stored.
        /// </summary>
        public const string HIT_COLLECTION = "hits";

        /// <summary>
        /// The name of the Mongo collection where <see cref="TruffleReports.Entities.ReportGenerationSummary"/>s are stored.
        /// </summary>
        public const string SUMMARY_COLLECTION = "summaries";

        /// <summary>
        /// The name of the Mongo database where TruffleReports stores its collections.
        /// </summary>
        public const string REPORTS_DATABASE = "trufflereports";
    }
}
