using System.Threading;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public static class Verifier
    {
        public static bool IsVerifyRequired(string requestPath)
        {
            var path = requestPath.ToLowerInvariant();
            return
                path.IndexOf(".asmx/js") <= 0
                && !path.EndsWith(".sjs")
                && !path.EndsWith(".css")
                && !path.EndsWith(".js")
                && !path.EndsWith(".png")
                && !path.EndsWith(".gif")
                && !path.EndsWith(".axd")
                && !path.EndsWith(".ico")
                && !path.EndsWith("msajaxjs")
                && !path.EndsWith("webformsjs")
                && !path.EndsWith(".svc");
        }

        public static void VerifyAgainstThreadData(string requestPath, long expectedStudyId)
        {
            if (!IsVerifyRequired(requestPath)) return;

            var medrioPrincipal = Thread.CurrentPrincipal as MedrioPrincipal;

            Assert(medrioPrincipal != null, "Can not get medrioPrincipal back from thread!");

            Assert(medrioPrincipal!.Study != null, "Can not get study back from thread!");

            Assert(medrioPrincipal!.Study!.ID == expectedStudyId, $"StudyId expected {expectedStudyId}, but got {medrioPrincipal.Study.ID}");
        }


        public static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new ValidationException(message);
            }
        }

    }
}
