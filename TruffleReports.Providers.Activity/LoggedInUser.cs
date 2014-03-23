using System;

namespace TruffleReports.Providers.Activity
{
    /// <summary>
    /// Describes a logged in user and their tracked activity.
    /// </summary>
    [Serializable]
    public sealed class LoggedInUser
    {
        /// <summary>
        /// Gets or sets the identity of the user.
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Gets or sets DateTime of the first hit for this session.
        /// </summary>
        public DateTime FirstHit { get; set; }

        /// <summary>
        /// Gets or sets DateTime of the last hit during the session so far.
        /// </summary>
        public DateTime LastHit { get; set; }

        /// <summary>
        /// Gets or sets total hits they have made during the session so far.
        /// </summary>
        public long TotalHits { get; set; }

        /// <summary>
        /// Gets or sets average time between hits for the session so far.
        /// </summary>
        public TimeSpan AveragePerHit { get; set; }
    }
}