using System.ComponentModel.DataAnnotations;
using Medrio.Infrastructure.Ioc.ConfigurationOption;

namespace Medrio.CspReport
{
    /// <summary>
    /// Specify buffer setting for violations before de-duped and pushed to storage.
    /// 2 settings, which ever come first will cause buffer flushed to the storage.
    /// 
    /// Bigger/longer buffer will have better result for de-dup, but cause more memory.
    /// </summary>
    public class CspReportOption : AutoBindOption<CspReportOption>
    {
        public override string SectionName => "Medrio:CspReportOption";

        /// <summary>
        /// How many violations will be saved in buffer before pushed  to storage
        /// </summary>
        [Required]
        [Range(0, 3000)]
        public int BufferSize { get; set; } = 1000;

        /// <summary>
        /// How long the violations will be saved in buffer before pushed to storage.
        /// </summary>
        [Required]
        [Range(0, 1800000)] // Max 30 minutes
        public double BufferTimeInMs { get; set; } = TimeSpan.FromMinutes(5).TotalMilliseconds;


    }
}
