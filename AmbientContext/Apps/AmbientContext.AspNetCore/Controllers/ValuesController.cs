using AmbientContext.Shared.DotNetStandardLib;
using AmbientContext.Shared.DotNetStandardLib.Models;
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
            return AuthHelper.GetCurrentStudyId();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{studyId}")]
        public async Task<long> Get(long studyId)
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            await AuthHelper.SetStudyAsync(studyId);
            await AsyncActor.DoSthAsync().ConfigureAwait(false);

            return AuthHelper.GetCurrentStudyId();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<long> Post([FromBody] string value)
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            return AuthHelper.GetCurrentStudyId();
        }
    }
}
