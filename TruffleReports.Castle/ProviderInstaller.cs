using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TruffleReports.Contracts;

namespace TruffleReports.Castle
{
    /// <summary>
    /// A <see cref="IWindsorInstaller"/> which will install all implementations of <see cref="IReportProvider"/> found in the bin folder.
    /// </summary>
    public class ProviderInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter("bin")).BasedOn<IReportProvider>().WithServiceFirstInterface());
        }
    }
}