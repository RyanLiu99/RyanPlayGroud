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
    public class DefaultController
    {
        private readonly ILogger<DefaultController> logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public object Index()
        {
            var s = $"Receive a request at  + {DateTime.Now}.  \r\n" +
                    $"Go to <a href='/swagger/'>/swagger/</a>";
            logger.LogInformation(s);
            return s;
        }
    }
}
