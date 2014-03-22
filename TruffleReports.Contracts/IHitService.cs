namespace TruffleReports.Contracts
{
    /// <summary>
    /// Describes a service that can be used to Log <see cref="Hit"/>s.
    /// </summary>
    public interface IHitService
    {
        /// <summary>
        /// Logs the specified <see cref="Hit"/>.
        /// </summary>
        /// <param name="hit">The hit.</param>
        void Log(Hit hit);
    }
}
