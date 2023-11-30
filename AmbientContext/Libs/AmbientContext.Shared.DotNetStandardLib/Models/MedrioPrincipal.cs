using System.Security.Principal;

namespace AmbientContext.Shared.DotNetStandardLib.Models
{
    public class MedrioPrincipal : GenericPrincipal
    {
        public MedrioPrincipal(User user, ResearchStudy? study) 
            : this(new GenericIdentity(user.UserName), study, user, new[] { "AuthenticatedUser" })
        {
        }


        internal MedrioPrincipal(IIdentity identity, ResearchStudy? study, User user, string[] roles)
            : base(identity, roles)
        {
            Study = study;
            User = user;
        }

        public ResearchStudy? Study { get; set; }

        public User User { get; }


    }
}

