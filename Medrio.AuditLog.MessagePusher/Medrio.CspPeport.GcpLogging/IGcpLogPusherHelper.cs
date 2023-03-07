using System.Collections.Generic;
using Google.Cloud.Logging.V2;
using Medrio.AuditLog.MessagePusher.Gcp.Configurations;
using Microsoft.Extensions.Options;

namespace Medrio.AuditLog.MessagePusher.Gcp
{
    internal interface IGcpLogPusherHelper
    {
        IDictionary<string, string> CreateEntryLabels(in LogName logName);

    }
}