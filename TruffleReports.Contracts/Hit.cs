using System;

namespace TruffleReports.Contracts
{
    /// <summary>
    /// Describes a Hit. A Hit is a visit to a Url that TruffleReports will then expose for applying aggregation and summary functions on in order to generate
    /// a report.
    /// </summary>
    [Serializable]
    public sealed class Hit
    {
        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> the hit was logged.
        /// </summary>
        public DateTime Logged { get; set; }

        /// <summary>
        /// Gets or sets the host the hit was logged on.
        /// </summary>
        /// <example>http://localhost.co.uk/</example>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the path the hit was logged on.
        /// </summary>
        /// <example>API/Management/Users</example>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the status code which was the reponse for the hit.
        /// </summary>
        /// <example>200</example>
        /// <remarks>This should only be a value from<see cref="System.Net.HttpStatusCode"/>.</remarks>
        public int StatusCode { get; set; }  
    }
}
