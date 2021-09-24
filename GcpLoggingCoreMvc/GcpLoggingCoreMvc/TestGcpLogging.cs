using Google.Cloud.Logging.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace GcpLoggingCoreMvc
{

    //PS  $env:GOOGLE_APPLICATION_CREDENTIALS = "C:\rliu-OneDrive\OneDrive\IT\GcpServiceAccountKey-ryantest1-4fd63-447661f46186.json"
    public class TestGcpLogging
    {
        public WriteLogEntriesResponse WriteLog()
        {
            // Your Google Cloud Platform project ID.
            string projectId = "ryantest1-4fd63";

            // Instantiates a client.
            var client = LoggingServiceV2Client.Create();

            //properties applied to all entries
            string logId = "my-gcp-code-log"; //will be folder name in storage bucket. 
            LogName logName = new LogName(projectId, logId); // will be logName prop for each logs in logging bucket, also prop in gcs bucket json file

            MonitoredResource resource = new MonitoredResource  //mandatory
            {
                Type = "global"
                //Labels prop is read only. But it will be populated with project_id automatically.
            };

            // Create dictionary object to add custom labels to the log entry.
            IDictionary<string, string> entryLabels = new Dictionary<string, string>();
            entryLabels.Add("size", "large");
            entryLabels.Add("color", "red");

            //----------
            LogEntry logEntry = new LogEntry()
            {
                LogNameAsLogName = logName,
                Severity = LogSeverity.Info,
                //TextPayload = $"{DateTime.Now}  Hello CGP!",
                Operation = new LogEntryOperation() { First = true, Producer = "Log Entry operation producer5"},
                //SourceLocation = Source code location information, file/line
                JsonPayload = CreateJsonPayLoad(new Exception("Test gcp log ex 5"), "Msg for json payload 5"),
                //Labels = { }, user defined, optional , can also set in batch
                //HttpRequest = {},
                //InsertId = {},
                //LogName  ={ },
                //ProtoPayload = ,
                //ReceiveTimestamp = , The time the log entry was received by Logging.
                //Resource=//, The monitored resource that produced this log entry. can set in the batch, which has Type property. GCP will auto add Resource.Lables(readonly) which contains project_id
                //SpanId = can read/write, no default
                //Timestamp =  Timestamp  the event described by the log entry occurred. If omitted, current time is used,
                Trace = "Optional, Resource name of the trace associated with the log entry, if any. Related to //tracing.googleapis.com",
                //TraceSampled = true,
            };
            
            var logEntry2 = new LogEntry()
            {
                LogNameAsLogName = logName,
                Severity = LogSeverity.Error,
                TextPayload = $"{DateTime.Now}  Hello CGP 5!",
                Operation = new LogEntryOperation() { Last = true, Producer = "Log Entry operation producer1" }
            };

            IEnumerable<LogEntry> logEntries = new LogEntry[] { logEntry, logEntry2 };

            // Write new log entry.
            WriteLogEntriesResponse response = client.WriteLogEntries(logName, resource, entryLabels, logEntries);

            Console.WriteLine("Log Entry created 5.");
            return response;
        }

        private Struct CreateJsonPayLoad( Exception exception, string message)
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

  

            return jsonStruct;
        }
    }
}
