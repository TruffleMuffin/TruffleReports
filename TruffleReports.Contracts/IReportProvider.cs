using System.Collections.Generic;
using System.Threading.Tasks;

namespace TruffleReports.Contracts
{
    /// <summary>
    /// Describes a provider that will receive periodic calls informing it about when <see cref="Hit"/>s have been logged for it to process.
    /// </summary>
    /// <remarks>Recommend that you use the <see cref="TruffleReports.Helpers.GeneratorHelper"/> to access useful properties.</remarks>
    public interface IReportProvider
    {
        /// <summary>
        /// Gets the unqiue name of the report the instance provides.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Generates a report
        /// </summary>
        /// <param name="hits">The hits.</param>
        /// <returns>
        /// A <see cref="TruffleReports.Contracts.ReportGenerationResult" /> regarding this instances running.
        /// </returns>
        Task<ReportGenerationResult> Generate(IEnumerable<Hit> hits);

        /// <summary>
        /// Gets report for the host with the specified queryString
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>A report</returns>
        Task<object> Get(string host, IEnumerable<KeyValuePair<string, string>> queryString);
    }
}