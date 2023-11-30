using System.Threading;
using System.Threading.Tasks;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public class AuthHelper
    {
        public static bool IsMainRequest(string requestPath)
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

        public static void SetThreadData(RequestData data)
        {
            var user = new User(data.UserName);
            var study = new ResearchStudy(data.StudyId);
            var principal = new MedrioPrincipal(user, study);
            Thread.CurrentPrincipal = principal;
            ThreadDataStore.AddObject(ThreadDataStoreCommonKeys.Study, study);
        }

        public static async Task SetStudyAsync(long studyId)
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            var principal = GetCurrentPrincipal();
            principal.Study = new ResearchStudy(studyId);
            ThreadDataStore.AddObject(ThreadDataStoreCommonKeys.Study, principal.Study);
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
        }

        public static MedrioPrincipal GetCurrentPrincipal()
        {
            return (MedrioPrincipal)Thread.CurrentPrincipal;
        }

        public static long GetCurrentStudyId()
        {
            return GetCurrentPrincipal().Study.ID;
        }

        public static async Task OverwriteStudyIdInManualTask(long newStudyId)
        {
            await Task.Run(async () =>
            {
                await AuthHelper.SetStudyAsync(newStudyId).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
