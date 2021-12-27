using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly IHttpClientFactory httpClientFactory;

        public DefaultController(ILogger<DefaultController> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<string> Index(string q)
        {
            using var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:252");
            var backEndResponse = await httpClient.GetStringAsync("?q=" + q).ConfigureAwait(false);
            var s = $"[{DateTime.Now}] Proxy {Environment.MachineName} Got from back end: [{backEndResponse}]";
            logger.LogInformation(s);
            return s;
        }
    }
}
