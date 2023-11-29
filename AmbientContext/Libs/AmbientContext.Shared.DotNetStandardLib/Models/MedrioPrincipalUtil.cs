using System;

namespace AmbientContext.Shared.DotNetStandardLib.Models
{
    public class MedrioPrincipalUtil
    {
        public static ResearchStudy? GetResearchStudy(bool shouldThrowExceptionIfStudyNotFound = true)
        {
            var medrioPrincipal = GetMedrioPrincipal(shouldThrowExceptionIfStudyNotFound);

            if (medrioPrincipal?.Study != null) return medrioPrincipal.Study;

            if (shouldThrowExceptionIfStudyNotFound)
                throw new ArgumentNullException();

            return null;
        }

        public static User? GetUser(bool shouldThrowExceptionIfUserNotFound = true)
        {
            var medrioPrincipal = GetMedrioPrincipal();

            if (medrioPrincipal?.User != null) return medrioPrincipal.User;

            if (shouldThrowExceptionIfUserNotFound)
                throw new ArgumentNullException();

            return null;
        }

        public static MedrioPrincipal? GetMedrioPrincipal(bool shouldThrowExceptionIfNotFound = true)
        {
            var medrioPrincipal = System.Threading.Thread.CurrentPrincipal as MedrioPrincipal;
            if (medrioPrincipal == null && shouldThrowExceptionIfNotFound)
                throw new ArgumentNullException();
            return medrioPrincipal;
        }
    }
}
