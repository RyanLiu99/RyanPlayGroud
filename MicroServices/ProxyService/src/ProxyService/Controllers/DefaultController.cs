using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProxyService.Controllers
{
    [Route("api/[controller]")]
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public object Index(string q)
        {
            var s = $"Proxy Service {Environment.MachineName} received a request at  + {DateTime.Now}.  {q} " +
                    $"Try /Index?q=echo back. " +
                    $"Go to /swagger/ if in Development mode";


            logger.LogInformation(s);
            return s;
        }
    }
}
