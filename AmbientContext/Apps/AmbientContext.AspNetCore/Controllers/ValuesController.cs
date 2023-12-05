using AmbientContext.Shared.DotNetStandardLib;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipelines;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AmbientContext_AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<long> Get()
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            return AuthHelper.GetCurrentStudyIdFromThread();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{studyId}")]
        public async Task<long> Get(long studyId)
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            await AuthHelper.ChangeThreadStudyAsync(studyId); //over write study from query string with value from route data
            await AsyncActor.DoSthAsync().ConfigureAwait(false);


            Verifier.VerifyThreadData(studyId);
            Verifier.VerifyStoreData(studyId);

            return AuthHelper.GetCurrentStudyIdFromThread();
        }


        //// POST api/<ValuesController>
        //[HttpPost]
        //public async Task<long> Post([FromBody] IFormFile file)
        //{
        //    var bodyLength = Request.ContentLength;

        //    file.OpenReadStream();
        //    ReadResult readResult = await Request.BodyReader.ReadAsync();
        //    var read = Encoding.UTF8.GetString(readResult.Buffer);
        //    Console.Write(read.Substring(0, Math.Min(5000, read.Length)));

        //    await AsyncActor.DoSthAsync().ConfigureAwait(false);
        //    return AuthHelper.GetCurrentStudyIdFromThread();
        //}

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<long> Post()
        {

            ReadResult readResult = await Request.BodyReader.ReadAsync();
            var read = Encoding.UTF8.GetString(readResult.Buffer);
            _logger.LogInformation("Read from post: " + read.Substring(0, Math.Min(5000, read.Length)));

            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            return AuthHelper.GetCurrentStudyIdFromThread();
        }
    }
}
