using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace AmbientContext.Shared.DotNetStandardLib.Models
{
    public class MedrioPrincipal : MedrioPrincipalOld
    {
        private readonly Dictionary<string, object> _dataBag = new (); //TODO: can be lazy inited.

        internal Dictionary<string, object> DataBag => _dataBag; 

        public MedrioPrincipal(User user, ResearchStudy? study) : base(user, study)
        {
        }

        internal MedrioPrincipal(IIdentity identity, ResearchStudy? study, User user, string[] roles) : base(identity, study, user, roles)
        {
        }

        
    }
}
