using System;

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
        /// Gets or sets <see cref="DateTime"/> this report was generated.
        /// </summary>
        public DateTime Generated { get; set; }

        /// <summary>
        /// Gets or sets the total number of logged in users.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the information tracked about the logged in users.
        /// </summary>
        public LoggedInUser[] Users { get; set; }
    }
}