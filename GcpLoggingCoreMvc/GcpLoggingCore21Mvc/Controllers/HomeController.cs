using GcpLoggingCore21Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GcpLoggingCore21Mvc.Controllers
{
    public class HomeController : Controller
    {
        public static readonly EventId TestGcpLoggingV2EventId = new EventId(5002, "TestGcpLoggingEventId");

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Log()
        {
            ViewData["Message"] = "Check GCP for new logs.";

            _logger.LogCritical(TestGcpLoggingV2EventId, 
                new Exception("Fake exception"), 
                "HomeController log a CriticalMsg: {criticalMsg}", 
                new CriticalMsg { Age = 55, CriticalStr = "Prop2" });

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    internal class CriticalMsg
    {
        public int Age { get; set; }
        public string CriticalStr { get; set; }

    }
}
