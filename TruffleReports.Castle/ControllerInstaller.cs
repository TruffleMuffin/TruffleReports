using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace TruffleReports.Castle
{
    /// <summary>
    /// A <see cref="IWindsorInstaller"/> which will install all implementations of <see cref="IHttpController"/> found in TruffleReports.
    /// </summary>
    public sealed class ControllerInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyNamed("TruffleReports").BasedOn<IHttpController>().LifestyleTransient());
        }
    }
}