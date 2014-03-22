namespace TruffleReports.Contracts
{
    /// <summary>
    /// Describes results that are possible from attempting to generate a report
    /// </summary>
    public enum ReportResult
    {
        /// <summary>
        /// The Report Generation was not run due to an unknown issue.
        /// </summary>
        NotRun,

        /// <summary>
        /// The Report Generation was run successfully.
        /// </summary>
        Success,

        /// <summary>
        /// The Report Generation was failed during with an unknown issue.
        /// </summary>
        UnknownFailure,

        /// <summary>
        /// The Report Generation was not run due due to a lack of <see cref="Hit"/>s regarding this type of Report.
        /// </summary>
        NotEnoughInformation
    }
}