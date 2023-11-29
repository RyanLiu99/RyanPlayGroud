using System.Threading;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public class AuthHelper
    {
        public static void SetThreadData(RequestData data)
        {
            var user = new User(data.UserName);
            var study = new ResearchStudy(data.StudyId);
            var principal = new MedrioPrincipal(user, study);
            Thread.CurrentPrincipal = principal;
        }
    }
}
