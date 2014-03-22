using System;

namespace TruffleReports.Contracts
{
    /// <summary>
    /// Describes a service for generating reports.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Generates reports, providing the startDate to scope the generation.
        /// </summary>
        /// <param name="startWindow">The start of the generation window.</param>
        /// <param name="endWindow">The end of the generation window.</param>
        void Generate(DateTime startWindow, DateTime endWindow);
    }
}