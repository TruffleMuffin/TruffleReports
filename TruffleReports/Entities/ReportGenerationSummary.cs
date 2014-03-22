using System;
using System.Collections.Generic;
using TruffleReports.Contracts;

namespace TruffleReports.Entities
{
    /// <summary>
    /// Describes the summary of a Report Generation sequence executed by <see cref="TruffleReports.Services.ReportService"/>.
    /// </summary>
    [Serializable]
    public sealed class ReportGenerationSummary
    {
        /// <summary>
        /// Gets or sets the datetime the report generation was run at.
        /// </summary>
        public DateTime RunAt { get; set; }

        /// <summary>
        /// Gets or sets the duration of time it took to execute the reports.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        public List<ReportGenerationResult> Results { get; set; }
    }
}
