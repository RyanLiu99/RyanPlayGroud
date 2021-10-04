using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GcpLoggingNet5MvcLogDirectlyAndILogger.Models;
using GcpLoggingNet5MvcLogDirectlyAndILogger.Services;
using Medrio.ActivityAuditLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GcpLoggingNet5MvcLogDirectlyAndILogger.Controllers
{
    public class HomeController : Controller
    {
        public static readonly EventId TestGcpLoggingEventId = new EventId(5000, "TestGcpLoggingEventId");

        private readonly ILogger<HomeController> _logger;
        private readonly IMyService _myService;
        private readonly IActivityAuditLogger _activityAuditLogger;

        public HomeController(ILogger<HomeController> logger, IMyService myService, IActivityAuditLogger activityAuditLogger)
        {
            if (myService is null)
            {
                throw new ArgumentNullException(nameof(myService));
            }

            _logger = logger;
            _myService = myService;
            _activityAuditLogger = activityAuditLogger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Task<string> Log()
        {
            return _activityAuditLogger.WriteLog(this.HttpContext);

            //var response = new DirectGcpLogging().WriteLog(this.HttpContext);   
            //_logger.LogCritical(TestGcpLoggingEventId, new Exception("Fake exception"), "2 HomeController ILogger log a CriticalMsg: {criticalMsg}", new CriticalMsg { Age = 55, CriticalStr = "Prop2"});
            ////_logger.LogInformation("In Controller, Activity.Current?.Id is {activityId}, HttpContextTraceId is {traceId}", Activity.Current?.Id, HttpContext.TraceIdentifier);
            //_myService.WriteSomeLog();
            
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

        public override string ToString()
        {
            return $"ToString - Age is {Age}, CriticalStr is {CriticalStr}";
        }
    }
}
