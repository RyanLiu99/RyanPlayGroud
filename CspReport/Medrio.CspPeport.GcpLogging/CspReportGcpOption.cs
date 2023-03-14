using System.ComponentModel.DataAnnotations;
using Medrio.Infrastructure.Ioc.ConfigurationOption;

namespace Medrio.CspReport.GcpLogging
{
    public class CspReportGcpOption : AutoBindOption<CspReportGcpOption>
    {
        public override string SectionName =>  new CspReportOption().SectionName + ":GCP";

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
    }
}
