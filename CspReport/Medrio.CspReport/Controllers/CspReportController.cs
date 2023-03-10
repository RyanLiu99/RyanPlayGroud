using Medrio.CspReport.GcpLogging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Medrio.CspReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CspReportController : ControllerBase
    {
        private readonly ICspReportPusher _pusher;

        public CspReportController(ICspReportPusher pusher)
        {
            _pusher = pusher;
        }


        [HttpGet("/")]
        public string Index() => "Welcome to Medrio CSP report site.";
        

        [EnableCors(Constants.Cors_CspReportPolicy)]
        [HttpPost("")]
        public Task Report()
        {
            return _pusher.PushReport(Request.Body);
        }
    }
}