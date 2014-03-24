using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TruffleReports.Services;

namespace TruffleReports.Api
{
    /// <summary>
    /// A API Endpoint for Reports queries.
    /// </summary>
    [RoutePrefix("API/Reports")]
    public class ReportsController : ApiController
    {
        private readonly ReportService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public ReportsController(ReportService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets a report of the specified type
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [Route("{name}")]
        public Task<object> Get(string name)
        {
            return service.Get(name, Request.RequestUri.Host, Request.GetQueryNameValuePairs());
        }
    }
}
