using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Cloud.Logging.V2;
using Google.Protobuf.WellKnownTypes;
using Medrio.AuditLog.MessagePusher.Gcp;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Medrio.CspReport.GcpLogging
{
    public interface ICspReportPusher
    {
        Task PushReport(Stream body);
    }

    [RegisterAs(typeof(ICspReportPusher), Lifetime = ServiceLifetime.Singleton)]
    internal class CspReportPusher : ICspReportPusher
    {
        private readonly ILogger<CspReportPusher> _logger;
        private readonly LoggingServiceV2Client _client;
        private readonly LogName _logName;
        private readonly MonitoredResource _resource;
        private readonly IDictionary<string, string> _entryLabels;

        public CspReportPusher(IOptions<CspReportGcpOption> gcpLogOptionAccessor, IGcpLogPusherHelper helper, ILogger<CspReportPusher> logger)
        {
            _logger = logger;
            CspReportGcpOption gcpLogOption = gcpLogOptionAccessor.Value;

            _resource = new MonitoredResource  //mandatory
            {
                Type = "global"
                //MonitoredResource.Labels prop is read only. But it will be populated with project_id automatically.
            };

            _logName = new LogName(gcpLogOption.GcpProjectId, gcpLogOption.GcpLogId);
            _entryLabels = helper.CreateEntryLabels(_logName);
            _client = LoggingServiceV2Client.Create();
        }

        public async Task PushReport(Stream body)
        {
            //TODO: optimize, push logs from  multiple posts, de-duplicate
            LogEntry[] batch = await BuildLogEntry(body);
            
            WriteLogEntriesResponse response = await _client.WriteLogEntriesAsync(
                _logName,
                _resource,
                _entryLabels,
                batch).ConfigureAwait(false);

            _logger.LogInformation("GCP client log response is {response}", response.ToString());
        }

        private async Task<LogEntry[]> BuildLogEntry(Stream body)
        {
            string? result = null;

            //if (body.CanSeek) body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(body))  //pass stream direct to ListValue.Parser cause error, let get string first
            {
                result = await reader.ReadToEndAsync();
            }

            try
            {
                var parsed = ListValue.Parser.ParseJson(result);
                return parsed.Values.Select(x => new LogEntry()
                {
                    Severity = LogSeverity.Info,
                    Timestamp = Timestamp.FromDateTime(DateTime.UtcNow), // must in UTC 
                    JsonPayload = x.StructValue
                }).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot parse json, upload as plain text.");
                return new[]
                {
                    new LogEntry()
                    {
                        Severity = LogSeverity.Info,
                        Timestamp = Timestamp.FromDateTime(DateTime.UtcNow), // must in UTC 
                        TextPayload = result
                    }
                };
            }
        }
    }
}