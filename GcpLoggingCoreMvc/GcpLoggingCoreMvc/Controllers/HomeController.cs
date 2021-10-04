using GcpLoggingCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GcpLoggingCoreMvc.Services;
using Medrio.ActivityAuditLog;

namespace GcpLoggingCoreMvc.Controllers
{
    public class HomeController : Controller
    {
        public static readonly EventId TestGcpLoggingEventId = new EventId(5000, "TestGcpLoggingEventId");

        private readonly ILogger<HomeController> _logger;
        private readonly IMyService _myService;
        private readonly IActivityAuditLog _activityAuditLog;

        public HomeController(ILogger<HomeController> logger, IMyService myService, IActivityAuditLog activityAuditLog)
        {
            if (myService is null)
            {
                throw new ArgumentNullException(nameof(myService));
            }

            _logger = logger;
            _myService = myService;
            _activityAuditLog = activityAuditLog;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  Task<string> Log()
        {
            return _activityAuditLog.WriteLog(this.HttpContext);

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
