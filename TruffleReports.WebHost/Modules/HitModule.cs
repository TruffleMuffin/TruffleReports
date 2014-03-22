using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TruffleReports.Contracts;
using TruffleReports.Contracts.IoC;

namespace TruffleReports.WebHost.Modules
{
    /// <summary>
    /// A HTTP Module which records <see cref="Hit"/>s.
    /// </summary>
    public sealed class HitModule : IHttpModule
    {
        private const string STOP_WATCH = "StopWatch";
        private static readonly EventHandlerTaskAsyncHelper startLogHelper = new EventHandlerTaskAsyncHelper(StartLog);
        private static readonly EventHandlerTaskAsyncHelper logHelper = new EventHandlerTaskAsyncHelper(Log);
        private static readonly IHitService service = Injector.Get<IHitService>();

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="app">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication app)
        {
            app.AddOnBeginRequestAsync(startLogHelper.BeginEventHandler, startLogHelper.EndEventHandler);
            app.AddOnEndRequestAsync(logHelper.BeginEventHandler, logHelper.EndEventHandler);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Starts the recording process for the <see cref="Hit"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static async Task StartLog(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            app.Context.Items[STOP_WATCH] = stopWatch;
        }

        /// <summary>
        /// Records the <see cref="Hit"/>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static async Task Log(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;

            var stopWatch = (Stopwatch)app.Context.Items[STOP_WATCH];
            stopWatch.Stop();

            var hit = new Hit
                {
                    Duration = new TimeSpan(stopWatch.ElapsedTicks),
                    Logged = DateTime.Now,
                    Identity = Thread.CurrentPrincipal.Identity.Name,
                    Host = app.Request.Url.Host,
                    Method = app.Request.HttpMethod,
                    Path = app.Request.Url.PathAndQuery,
                    UserAgent = app.Request.UserAgent,
                    StatusCode = app.Response.StatusCode,
                    SubStatusCode = app.Response.SubStatusCode
                };

            service.Log(hit);
        }
    }
}
