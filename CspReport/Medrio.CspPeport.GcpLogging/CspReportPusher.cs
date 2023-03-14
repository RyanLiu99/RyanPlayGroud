using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Cloud.Logging.V2;
using Google.Protobuf.WellKnownTypes;
using Medrio.BulkDataChannel;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Medrio.CspReport.GcpLogging
{

    [RegisterAs(typeof(ICspReportPusher), Lifetime = ServiceLifetime.Singleton)]
    internal class CspReportGcpPusher : ICspReportPusher
    {
        private readonly ILogger<CspReportGcpPusher> _logger;
        private readonly LoggingServiceV2Client _client;
        private readonly LogName _logName;
        private readonly MonitoredResource _resource;
        private readonly IDictionary<string, string> _entryLabels;

        public CspReportGcpPusher(
            IOptions<CspReportOption> cspReportOptionAccessor,
            IOptions<CspReportGcpOption> gcpLogOptionAccessor, 
            IGcpLogPusherHelper helper,
            ILogger<CspReportGcpPusher> logger)
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

            var bufferTime = TimeSpan.FromMilliseconds(cspReportOptionAccessor.Value.BufferTimeInMs);
        }

        public async Task Push(IList<string> violations)
        {
            LogEntry[] batch = violations.SelectMany(BuildLogEntry).ToArray();

            WriteLogEntriesResponse response = await _client.WriteLogEntriesAsync(
                _logName,
                _resource,
                _entryLabels,
                batch).ConfigureAwait(false);

            _logger.LogInformation("GCP client log response is {response}", response.ToString());
        }

        private LogEntry[] BuildLogEntry(string violation)
        {
            try
            {
                ListValue? parsed = ListValue.Parser.ParseJson(violation);
                return parsed.Values.Select((Value x) => new LogEntry()
                {
                    Severity = LogSeverity.Info,
                    //Timestamp = Timestamp.FromDateTime(DateTime.UtcNow), // must in UTC 
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
                        Severity = LogSeverity.Warning,
                        TextPayload = violation
                    }
                };
            }
        }
    }
}