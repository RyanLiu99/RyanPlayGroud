using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace AmbientContext.Shared.DotNetStandardLib.Models
{

    // MedrioPrincipalOld is not used directly. 2 classes is just to show the difference explicitly.
    //Otherwise can just add DataBag into MedrioPrincipalOld and use one class
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
