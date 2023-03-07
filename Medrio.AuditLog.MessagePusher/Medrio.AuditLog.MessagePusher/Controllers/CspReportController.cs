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
        private readonly ILogger<WeatherForecastController> _logger;
        private static List<string> _reports = new List<string>();

        public CspReportController(ICspReportPusher pusher, ILogger<WeatherForecastController> logger)
        {
            _pusher = pusher;
            _logger = logger;
        }

        [EnableCors(Constants.Cors_CspReportPolicy)]
        [HttpPost("")]
        public async Task<string> Report()
        {
            //if(Request.Body.CanSeek) Request.Body.Seek(0, SeekOrigin.Begin);
            //using var reader = new StreamReader(Request.Body);
            //var result = await reader.ReadToEndAsync();

            _reports.Add("Reported once");

            await _pusher.PushReport(Request.Body);

            return "Received";
        }

        [HttpGet("Reports")]
        public List<string> Reports()
        {
            return _reports;
        }

        [HttpGet("Reset")]
        public int Reset()
        {
            int c = _reports.Count;
            _reports.Clear();
            return c;
        }
    }
}