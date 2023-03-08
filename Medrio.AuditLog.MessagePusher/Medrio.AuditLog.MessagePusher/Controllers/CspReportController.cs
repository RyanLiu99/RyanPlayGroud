using Medrio.CspReport.GcpLogging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Medrio.AuditLog.MessagePusher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CspReportController : ControllerBase
    {
        private readonly ICspReportPusher _pusher;

        public CspReportController(ICspReportPusher pusher, ILogger<WeatherForecastController> logger)
        {
            _pusher = pusher;
        }

        [EnableCors(Constants.Cors_CspReportPolicy)]
        [HttpPost("")]
        public Task Report()
        {
            return _pusher.PushReport(Request.Body);
        }
    }
}