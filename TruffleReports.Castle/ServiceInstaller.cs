using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TruffleReports.Contracts;
using TruffleReports.Providers.Activity;
using TruffleReports.Services;

namespace TruffleReports.Castle
{
    /// <summary>
    /// A <see cref="IWindsorInstaller"/> which will install Services for TruffleReports.
    /// </summary>
    public sealed class ServiceInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDB.Connection"] ?? new ConnectionStringSettings("connectionString", "mongodb://localhost");
            var defaultDatabase = ConfigurationManager.ConnectionStrings["MongoDB.Database"] ?? new ConnectionStringSettings("defaultDatabase", "test");

            container.Register(
                Component
                    .For<IHitService>()
                    .ImplementedBy<HitService>()
                    .DependsOn(
                        Dependency.OnValue("connectionString", connectionString.ConnectionString),
                        Dependency.OnValue("defaultDatabase", defaultDatabase.ConnectionString)
                    ),

                Component
                    .For<IReportProvider>()
                    .ImplementedBy<LoggedInProvider>()
                    .DependsOn(
                        Dependency.OnValue("connectionString", connectionString.ConnectionString),
                        Dependency.OnValue("defaultDatabase", defaultDatabase.ConnectionString)
                    )
            );

            container.Register(
                Component
                    .For<IReportService>()
                    .ImplementedBy<ReportService>()
                    .DependsOn(
                        Dependency.OnValue("connectionString", connectionString.ConnectionString),
                        Dependency.OnValue("defaultDatabase", defaultDatabase.ConnectionString)
                    )
                    .DynamicParameters((k, p) => p.Add("providers", k.ResolveAll<IReportProvider>())));
        }
    }
}
