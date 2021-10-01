using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GcpLoggingCoreMvc.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GcpLoggingCoreMvc.Services
{
    public class MyService : IMyService
    {
        private readonly ILogger<MyService> _logger;
        public MyService(ILogger<MyService> logger)
        {
            _logger = logger;
        }
        public void WriteSomeLog()
        {
            _logger.LogWarning(HomeController.TestGcpLoggingEventId, "3 In MyService, ILogger logs an anonymous obj: {msObj}", new { MsgObjProp1 = 55, MsgObjPro2 = "Prop2" });
            //_logger.LogInformation("In MyService, Activity.Current?.Id is {activityId}", Activity.Current?.Id);
        }
    }
}
