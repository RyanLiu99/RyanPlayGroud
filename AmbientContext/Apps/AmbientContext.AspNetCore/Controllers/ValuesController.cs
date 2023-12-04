using AmbientContext.Shared.DotNetStandardLib;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AmbientContext_AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
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

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<long> Post()
        {
            var body = Request.ContentLength;
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            return AuthHelper.GetCurrentStudyIdFromThread();
        }
    }
}
