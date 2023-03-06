using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Net6Web1.Controllers
{
    [ApiController]
    
    [Route("[controller]")]
    public class CspReportController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private static List<string> _reports = new List<string>();


        public CspReportController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }



        [EnableCors(Constants.Cors_CspReportPolicy)]
        [HttpPost("")]
        public async Task<string> Report()
        {
            if(Request.Body.CanSeek) Request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(Request.Body);
            var result = await reader.ReadToEndAsync();
            _reports.Add(result);


            //ReadResult readResult;

            //do
            //{
            //    readResult = await Request.BodyReader.ReadAsync().ConfigureAwait(false);
            //    readResult.Buffer

            //} while (readResult.IsCompleted);
            return "Added";
        }


        [HttpGet("Reports")]
        public List<string> Reports()
        {
            return _reports;
        }

        [HttpGet("Reset")]
        public int Reset()
        {
            _reports.Clear();
            return _reports.Count;
        }

    }
}