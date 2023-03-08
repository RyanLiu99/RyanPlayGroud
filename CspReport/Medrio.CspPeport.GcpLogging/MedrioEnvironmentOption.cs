using System.ComponentModel.DataAnnotations;
using Medrio.Infrastructure.Ioc.ConfigurationOption;

namespace Medrio.CspPeport.GcpLogging
{
    public class MedrioEnvironmentOption : AutoBindOption<MedrioEnvironmentOption>
    {
        public override string SectionName => "Medrio";

        //Not needed for now
        ///// <summary>
        ///// EnvironmentType, Like Production; Stage; Development; Preview ...etc
        ///// </summary>
        //[Required]
        //public string EnvironmentType { get; set; } = "Development";

        /// <summary>
        /// Environment, like  Production; Stage3, Stage5 under Stage; Local, Dev-01, Devg-bran-06, Dev-bran-04 under Development... etc
        /// </summary>
        [Required]
        public string Environment { get; set; } = "Local";

        /// <summary>
        /// like na, eu, ap, optional. 
        /// </summary>
        public string Region { get; set; }
    }
}