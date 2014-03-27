using System;
using Newtonsoft.Json;

namespace TruffleReports.Providers.Activity
{
    /// <summary>
    /// Describes a segement of the <see cref="LoggedInReport"/>.
    /// </summary>
    [Serializable]
    public sealed class LoggedInSegment
    {
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