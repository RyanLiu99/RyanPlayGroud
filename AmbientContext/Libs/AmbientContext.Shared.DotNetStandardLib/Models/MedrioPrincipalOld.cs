using System.Security.Principal;

namespace AmbientContext.Shared.DotNetStandardLib.Models
{
    public class MedrioPrincipalOld : GenericPrincipal
    {
        public MedrioPrincipalOld(User user, ResearchStudy? study) 
            : this(new GenericIdentity(user.UserName), study, user, new[] { "AuthenticatedUser" })
        {
        }


        internal MedrioPrincipalOld(IIdentity identity, ResearchStudy? study, User user, string[] roles)
            : base(identity, roles)
        {
            Study = study;
            User = user;
        }

        public ResearchStudy? Study { get; set; }

        public User User { get; }


    }
}

