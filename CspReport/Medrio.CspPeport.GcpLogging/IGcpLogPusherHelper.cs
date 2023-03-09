using Google.Cloud.Logging.V2;

namespace Medrio.CspReport.GcpLogging
{
    internal interface IGcpLogPusherHelper
    {
        IDictionary<string, string> CreateEntryLabels(in LogName logName);

    }
}