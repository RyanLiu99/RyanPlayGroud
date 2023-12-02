using System.Threading;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public static class Verifier
    {
        public static void VerifyContextData(long expectedStudyId)
        {
            var medrioPrincipal = Thread.CurrentPrincipal as MedrioPrincipal;

            Assert(medrioPrincipal != null, "Can not get medrioPrincipal back from thread!");

            Assert(medrioPrincipal!.Study != null, "Can not get study back from thread!");

            Assert(medrioPrincipal!.Study!.ID == expectedStudyId, $"StudyId expected to be {expectedStudyId}, but got {medrioPrincipal.Study.ID}");  // +1 to test

            //var studyInStore = ThreadDataStore.GetObject<ResearchStudy>(ThreadDataStoreCommonKeys.Study);
            //Assert(studyInStore != null, "Can not get study back from store!");

            //Assert(studyInStore!.ID == expectedStudyId, $"StudyId expected to be {expectedStudyId} in store, but got {medrioPrincipal.Study.ID}");  // +1 to test
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
