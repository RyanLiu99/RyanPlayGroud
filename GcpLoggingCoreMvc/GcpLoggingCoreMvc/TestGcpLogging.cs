using Google.Cloud.Logging.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Protobuf.Collections;

namespace GcpLoggingCoreMvc
{

    //PS  $env:GOOGLE_APPLICATION_CREDENTIALS = "C:\rliu-OneDrive\OneDrive\IT\Google service account key when generate it ask to save  ryantest1-4fd63-447661f46186.json"
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

            MonitoredResource resource = new MonitoredResource
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
                TextPayload = $"{DateTime.Now.Millisecond}  Hello CGP!",
                Operation = new LogEntryOperation() { First = true, Producer = "Log Entry operation producer1"},
                //SourceLocation = Source code location information, file/line
                //JsonPayload = "",
                //Labels = { }, user defined, optional , can also set in batch
                //HttpRequest = {},
                //InsertId = {},
                //LogName  ={ },
                //ProtoPayload = ,
                //ReceiveTimestamp = , The time the log entry was received by Logging.
                //Resource=//, The monitored resource that produced this log entry. can set in the batch, which has Type property. GCP will auto add Resource.Lables(readonly) which contains project_id
                //SpanId = can read/write, no default
                //Timestamp =  Timestamp  the event described by the log entry occurred. If omitted, current time is used,
                Trace = "Resource name of the trace associated with the log entry, if any. ",
                //TraceSampled = true,
            };
            
            var logEntry2 = new LogEntry()
            {
                LogNameAsLogName = logName,
                Severity = LogSeverity.Error,
                TextPayload = $"{DateTime.Now.Millisecond}  Hello CGP 2!",
                Operation = new LogEntryOperation() { Last = true, Producer = "Log Entry operation producer1" }
            };

            IEnumerable<LogEntry> logEntries = new LogEntry[] { logEntry, logEntry2 };

            // Write new log entry.
            WriteLogEntriesResponse response = client.WriteLogEntries(logName, resource, entryLabels, logEntries);

            Console.WriteLine("Log Entry created.");
            return response;
        }
    }
}
