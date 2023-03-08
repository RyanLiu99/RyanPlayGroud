using Google.Cloud.Logging.V2;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Medrio.CspPeport.GcpLogging
{
    [RegisterAs(typeof(IGcpLogPusherHelper))]
    internal class GcpLogPusherHelper : IGcpLogPusherHelper
    {
        private readonly ILogger<GcpLogPusherHelper> _logger;
        
        private readonly MedrioEnvironmentOption _medrioEnvironmentOption;

        public GcpLogPusherHelper(
            IOptions<MedrioEnvironmentOption> medrioEnvironmentOptionAccessor,
            ILogger<GcpLogPusherHelper> logger)
        {
            _logger = logger;
            _medrioEnvironmentOption = medrioEnvironmentOptionAccessor.Value;
        }

        public IDictionary<string, string> CreateEntryLabels(in LogName logName)
        {
            var entryLabels = new Dictionary<string, string>();
            var environment = _medrioEnvironmentOption.Environment.Trim();
            entryLabels.Add("Environment", environment);

            var region = _medrioEnvironmentOption.Region?.Trim();
            if (!string.IsNullOrWhiteSpace(region))
            {
                entryLabels.Add("Region", region);
            }

            _logger.LogInformation("Start pushing logs to GCP Project {projectId}, logName {logName}, Environment: {Environment}, Region: {Region}.",
                logName.ProjectId, logName.LogId, environment, region);

            return entryLabels;
        }
    }
}
