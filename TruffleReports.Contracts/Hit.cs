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
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> the <see cref="Hit"/> was logged.
        /// </summary>
        public DateTime Logged { get; set; }

        /// <summary>
        /// Gets or sets the host the <see cref="Hit"/> was logged on.
        /// </summary>
        /// <example>http://localhost.co.uk/</example>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the path the <see cref="Hit"/> was logged on.
        /// </summary>
        /// <example>API/Management/Users</example>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the status code which was the reponse for the <see cref="Hit"/>.
        /// </summary>
        /// <example>200</example>
        /// <remarks>This should only be a value from <see cref="System.Net.HttpStatusCode"/>.</remarks>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the sub status code which was the reponse for the <see cref="Hit"/>.
        /// </summary>
        /// <example>1</example>
        /// <remarks>See <seealso cref="http://support.microsoft.com/?scid=kb%3Ben-us%3B318380&x=17&y=8"/> for full list.</remarks>
        public int SubStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the duration of the <see cref="Hit"/>.
        /// </summary>
        /// <remarks>The <see cref="TimeSpan"/> between the BeginRequest and the EndRequest.</remarks>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the identity of the user that made the <see cref="Hit"/>.
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Gets or sets the user agent the <see cref="Hit"/> was made with.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the http method for the <see cref="Hit"/>.
        /// </summary>
        /// <example>HEAD|OPTIONS|GET|PUT|DELETE|POST</example>
        /// <remarks>This should only be a value from the <see cref="System.Net.Http.HttpMethod"/> properties list.</remarks>
        public string Method { get; set; }
    }
}
