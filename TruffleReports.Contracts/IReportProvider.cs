using System;
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
        /// <param name="startWindow">The start of the generation window.</param>
        /// <param name="endWindow">The end of the generation window.</param>
        /// <returns>
        /// A <see cref="TruffleReports.Contracts.ReportGenerationResult" /> regarding this instances running.
        /// </returns>
        Task<ReportGenerationResult> Generate(DateTime startWindow, DateTime endWindow);
    }
}