using System.ComponentModel.DataAnnotations;
using Medrio.Infrastructure.Ioc.ConfigurationOption;

namespace Medrio.CspReport.GcpLogging
{
    public class CspReportGcpOption : AutoBindOption<CspReportGcpOption>
    {
        public override string SectionName => "Medrio:CspReportGcpOption";

        /// <summary>
        /// Google Cloud Platform project ID.
        /// </summary>
        [Required]
        public string GcpProjectId { get; set; } = "ryantest1-4fd63"; // "devsite-223019"  "ryantest1-4fd63";

        /// <summary>
        /// Log to save csp report
        /// </summary>
        /// <remarks>will be folder name in storage bucket. </remarks>
        [Required]
        public string GcpLogId { get; set; } = "Csp_Report";

        /// <summary>
        /// Limit how big is the payload in one log entry.
        /// GCP log entry has limit of 256K. Leave some space for meta data, headers, labels etc. And encoding could double the size
        /// </summary>
        public int RequestBodyChunkSize { get; set; } = 100000;
        
        private int _batchSize = 20;
        /// <summary>
        /// Limit how many log entries in one call to GCP
        /// For big http request, we are going to divide to small chunks <see cref="RequestBodyChunkSize"/> to avoid GCP log entry size limit.
        /// GCP API also has a limit how big the call is,  so for this case, we also need limit how many chunks we can send at a time. Should not exceed 10M/256K = 39.
        /// </summary>
        public int BathSize
        {
            get => _batchSize;
            set => _batchSize = value > 39 ? 39 : (value <= 0 ? _batchSize : value);
        }
    }
}
