using Google.Cloud.Logging.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Api;
using Google.Cloud.Logging.Type;

namespace GcpLoggingCoreMvc
{
  

    public class TestGcpLogging
    {
        public WriteLogEntriesResponse WriteLog()
        {
            // Your Google Cloud Platform project ID.
            string projectId = "ryantest1-4fd63";

            // Instantiates a client.
            var client = LoggingServiceV2Client.Create();

            // Prepare new log entry.
            LogEntry logEntry = new LogEntry();
            string logId = "my-gcp-code-log";
            LogName logName = new LogName(projectId, logId);
            logEntry.LogNameAsLogName = logName;
            logEntry.Severity = LogSeverity.Info;

            // Create log entry message.
            string message = "Hello CGP!";
            string messageId = DateTime.Now.Millisecond.ToString();
            Type myType = typeof(TestGcpLogging);
            string entrySeverity = logEntry.Severity.ToString().ToUpper();
            logEntry.TextPayload =
                $"{messageId} {entrySeverity} {myType.Namespace}.LoggingSample - {message}";

       
            MonitoredResource resource = new MonitoredResource
            {
                Type = "global"
            };

            // Create dictionary object to add custom labels to the log entry.
            IDictionary<string, string> entryLabels = new Dictionary<string, string>();
            entryLabels.Add("size", "large");
            entryLabels.Add("color", "red");

            // Add log entry to collection for writing. Multiple log entries can be added.
            IEnumerable<LogEntry> logEntries = new LogEntry[] { logEntry };

            // Write new log entry.
            WriteLogEntriesResponse response = client.WriteLogEntries(logName, resource, entryLabels, logEntries);

            Console.WriteLine("Log Entry created.");
            return response;
        }
    }
}
