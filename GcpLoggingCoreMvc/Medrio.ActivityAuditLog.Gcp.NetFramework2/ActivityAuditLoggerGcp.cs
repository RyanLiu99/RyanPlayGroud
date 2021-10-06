using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Cloud.Logging.V2;
using Google.Protobuf.WellKnownTypes;
using Medrio.ActivityAuditLog.NetFramework;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Medrio.ActivityAuditLog.Gcp.NetFramework
{

    //PS  $env:GOOGLE_APPLICATION_CREDENTIALS = "C:\gcp\GcpServiceAccountKey-ryantest1-4fd63-447661f46186.json"
    //> gcloud logging logs delete Activity_Audit_Log
    //filer: logName="projects/ryantest1-4fd63/logs/Activity_Audit_Log"
    //For Big query , Fields must contain only letters, numbers, and underscores. So change - to _ first for now.

    [RegisterAs(typeof(IActivityAuditLogger), Lifetime = ServiceLifetime.Scoped)]
    public class ActivityAuditLoggerGcp : IActivityAuditLogger
    {
        // Your Google Cloud Platform project ID.
        private static string projectId = "ryantest1-4fd63";
        static string logId = "Activity_Audit_Log"; //will be folder name in storage bucket. 
        private static readonly LogName LogName = new LogName(projectId, logId); // will be logName prop for each logs in logging bucket, also prop in gcs bucket json file
        private readonly ILogRequestBuilder<Struct> _logRequestBuilder;
        private static readonly LogSeverity Severity = LogSeverity.Info;

        private static volatile int _c = 0;

        public ActivityAuditLoggerGcp(ILogRequestBuilder<Struct> logRequestBuilder)
        {
            _logRequestBuilder = logRequestBuilder;
        }
        public async Task<string> WriteLog(HttpContext context)
        {
            if (context == null) return null;

            var logRequest = await _logRequestBuilder.BuildLogRequest(context)
                .ConfigureAwait(false);

            // Create dictionary object to add custom labels to the log entry.
            IDictionary<string, string> entryLabels = new Dictionary<string, string>();
            entryLabels.Add("customerId", logRequest.CustomerId + _c++);
            entryLabels.Add("studyId", logRequest.StudyId);
            entryLabels.Add("app", "MedrioWeb");

            // Instantiates a client.
            var client = await LoggingServiceV2Client.CreateAsync().ConfigureAwait(false);

            MonitoredResource resource = new MonitoredResource  //mandatory
            {
                Type = "global"
                //Labels prop is read only. But it will be populated with project_id automatically.
            };


            LogEntry logEntry = new LogEntry()
            {
                LogNameAsLogName = LogName,
                Severity = Severity,
                Operation = new LogEntryOperation() { First = true, Producer = "DirectGcpLogging"},
                #region comments
                //TextPayload = $"{DateTime.Now}  Hello CGP!",
                //SourceLocation = Source code location information, file/line
                //Labels = { }, user defined, optional , can also set in batch
                //HttpRequest = {},
                //InsertId = {},
                //LogName  ={ },
                //ProtoPayload = ,
                //ReceiveTimestamp = , The time the log entry was received by Logging.
                //Resource=//, The monitored resource that produced this log entry. can set in the batch, which has Type property. GCP will auto add Resource.Lables(readonly) which contains project_id
                //SpanId = can read/write, no default
                //Timestamp =  Timestamp  the event described by the log entry occurred. If omitted, current time is used,
                //TraceSampled = true,
                //SourceLocation = //Optional. Source code location information associated with the log entry, if any.
                //Trace = "Optional, Resource name of the trace associated with the log entry, if any. Related to //tracing.googleapis.com",
                #endregion

                JsonPayload = logRequest.PayLoad
            };
            

            IEnumerable<LogEntry> logEntries = new LogEntry[] { logEntry};

            // Write new log entry.
            WriteLogEntriesResponse response = await client.WriteLogEntriesAsync(LogName, resource, entryLabels, logEntries).ConfigureAwait(false);

            Console.WriteLine("Log Entries sent, response is " + response );
            return string.Empty;
        }

        
        //private HttpRequest GetHttpRequest(HttpContext httpContext)
        //{
        //    //properly we add our httpRequest in json payload, not user Google httpRequest property and type which is very limited. 
        //    //if we not set Google httpRequest property, it then won't show up in log.
        //    return new HttpRequest()
        //    {
        //        //Referer =
        //        RequestMethod = httpContext.Request.Method,
        //        Protocol = httpContext.Request.Protocol,
        //        RemoteIp = httpContext.Connection.RemoteIpAddress.ToString()
        //    };
        //}

        
    }
}
