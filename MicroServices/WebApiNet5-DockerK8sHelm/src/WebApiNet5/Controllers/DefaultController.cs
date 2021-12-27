using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNet5.Controllers
{
    [ApiController]
    [Route("/[action]")]
    [Route("")]
    public class DefaultController
    {
        private readonly ILogger<DefaultController> logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public object Index(string q)
        {
            var s = $"WebApiNet5 BackEnd service {Environment.MachineName} received a request at  + {DateTime.Now}.  {q} " +
                    $"Try /Index?q=echo back. " +
                    $"Go to /swagger/ if in Development mode";
            logger.LogInformation(s);
            return s;
        }
    }
}
