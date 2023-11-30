using System.Threading;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public static class Verifier
    {
        public static void VerifyThreadData(long expectedStudyId)
        {
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
