
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
        public const string HIT_COLLECTION = "truffle_reports_hits";

        /// <summary>
        /// The name of the Mongo collection where <see cref="TruffleReports.Entities.ReportGenerationSummary"/>s are stored.
        /// </summary>
        public const string SUMMARY_COLLECTION = "truffle_reports_summaries";
    }
}
