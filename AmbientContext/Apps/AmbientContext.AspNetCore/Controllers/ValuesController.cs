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
        public async Task<object> Get()
        {
            var principal = (MedrioPrincipal)Thread.CurrentPrincipal;
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            return new { userName = principal.User.UserName, studyId = principal.Study.ID };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{studyId}")]
        public async Task<object> Get(string userName, long studyId)
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            var principal = (MedrioPrincipal)Thread.CurrentPrincipal;
            var oldStudyId = principal?.Study?.ID;
            principal.Study.ID = studyId;
            return new { userName= principal.User.UserName, studyId= principal.Study.ID, oldStudyId };
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
