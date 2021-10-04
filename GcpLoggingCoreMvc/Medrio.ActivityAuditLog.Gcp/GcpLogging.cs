using Google.Cloud.Logging.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Protobuf.WellKnownTypes;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Medrio.ActivityAuditLog.Gcp
{

    //PS  $env:GOOGLE_APPLICATION_CREDENTIALS = "C:\gcp\GcpServiceAccountKey-ryantest1-4fd63-447661f46186.json"
    //> gcloud logging logs delete Activity_Audit_Log
    //filer: logName="projects/ryantest1-4fd63/logs/Activity_Audit_Log"
    //For Big query , Fields must contain only letters, numbers, and underscores. So change - to _ first for now.

    [RegisterAs(typeof(IActivityAuditLog), Lifetime = ServiceLifetime.Scoped)]
    public class GcpLogging : IActivityAuditLog
    {
        // Your Google Cloud Platform project ID.
        private string projectId = "ryantest1-4fd63";
        string logId = "Activity_Audit_Log"; //will be folder name in storage bucket. 

        public Task<string> WriteLog(HttpContext httpContext)
        {
            Console.WriteLine("Headers ToString() String is " + httpContext.Request.Headers.ToString());

            
            // Instantiates a client.
            var client = LoggingServiceV2Client.Create();

            //properties applied to all entries
            
            LogName logName = new LogName(projectId, logId); // will be logName prop for each logs in logging bucket, also prop in gcs bucket json file

            MonitoredResource resource = new MonitoredResource  //mandatory
            {
                Type = "global"
                //Labels prop is read only. But it will be populated with project_id automatically.
            };

            // Create dictionary object to add custom labels to the log entry.
            IDictionary<string, string> entryLabels = new Dictionary<string, string>();
            entryLabels.Add("customerId", "customer-1");
            entryLabels.Add("studyId", "StudyId-100");
            entryLabels.Add("app", "MedrioWeb");

            //----------
            LogEntry logEntry = new LogEntry()
            {
                LogNameAsLogName = logName,
                Severity = LogSeverity.Info,
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

                //HttpRequest = GetHttpRequest(httpContext),
                JsonPayload = CreateJsonPayLoad(
                    httpContext,
                    new Exception("1.1 Direct API call fake exception!"),
                    "1.1 Direct API call Msg for json payload!")
            };
            
            var logEntry2 = new LogEntry()
            {
                LogNameAsLogName = logName,
                Severity = LogSeverity.Error,
                Operation = new LogEntryOperation() { Last = true, Producer = "DirectGcpLogging Log Entry operation Producer" },
               //HttpRequest = GetHttpRequest(httpContext),
                TextPayload = $"1.2 Direct API call text payload!",
            };

            IEnumerable<LogEntry> logEntries = new LogEntry[] { logEntry, logEntry2 };

            // Write new log entry.
            WriteLogEntriesResponse response = client.WriteLogEntries(logName, resource, entryLabels, logEntries);

            Console.WriteLine("Log Entries sent, response is " + response );
            return Task.FromResult(null as string);
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

        private Struct CreateJsonPayLoad(HttpContext httpContext, Exception exception, string message)
        {
            EventId eventId = new EventId(88, "TestGcpLoggingEventId");
            var jsonStruct = new Struct();
            jsonStruct.Fields.Add("message", Value.ForString(message));
            
            //if (_loggerOptions.ServiceContext != null)
            //{
            //    jsonStruct.Fields.Add("serviceContext", Value.ForStruct(_loggerOptions.ServiceContext));
            //}
            if (exception != null)
            {
                jsonStruct.Fields.Add("exception", Value.ForString(exception.ToString()));
            }

            if (eventId.Id != 0 || eventId.Name != null)
            {
                var eventStruct = new Struct();
                if (eventId.Id != 0)
                {
                    eventStruct.Fields.Add("id", Value.ForNumber(eventId.Id));
                }
                if (!string.IsNullOrWhiteSpace(eventId.Name))
                {
                    eventStruct.Fields.Add("name", Value.ForString(eventId.Name));
                }
                jsonStruct.Fields.Add("event_id", Value.ForStruct(eventStruct));
            }

            var requestStruct = new Struct();
            var requestHeaderStruct = new Struct();
            foreach (var header in httpContext.Request.Headers)
            {
                requestHeaderStruct.Fields.Add(
                    header.Key.Replace('-','_').Replace(':', '_'),  //For Big query , Fields must contain only letters, numbers, and underscores. Not start with letter.  Use regex or use data flow clean up data before import to big query
                    header.Value.Count > 1 ?
                    Value.ForList(header.Value.Select(Value.ForString).ToArray()) :
                    Value.ForString(header.Value)
                    );
            }

            requestStruct.Fields.Add("header", Value.ForStruct(requestHeaderStruct));
            jsonStruct.Fields.Add("request", Value.ForStruct(requestStruct));

            return jsonStruct;
        }
        

        
    }
}
