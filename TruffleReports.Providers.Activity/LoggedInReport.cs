using System;
using System.Collections.Generic;

namespace TruffleReports.Providers.Activity
{
    /// <summary>
    /// Describes a report for currently logged in users.
    /// </summary>
    [Serializable]
    public sealed class LoggedInReport
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date the report applies to.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the host this report is for.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the segments for this date.
        /// </summary>
        public List<LoggedInSegment> Segments { get; set; }
    }
}