using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TruffleReports.Contracts
{
    /// <summary>
    /// Describes a provider that will receive periodic calls informing it about when <see cref="Hit"/>s have been logged for it to process.
    /// </summary>
    /// <remarks>Recommend that you use the <see cref="TruffleReports.Providers.BaseReportProvider"/> and extend it.</remarks>
    public interface IReportProvider
    {
        /// <summary>
        /// Generates a report
        /// </summary>
        /// <param name="hits">The hits.</param>
        /// <returns>
        /// A <see cref="TruffleReports.Contracts.ReportGenerationResult" /> regarding this instances running.
        /// </returns>
        Task<ReportGenerationResult> Generate(IEnumerable<Hit> hits);
    }
}