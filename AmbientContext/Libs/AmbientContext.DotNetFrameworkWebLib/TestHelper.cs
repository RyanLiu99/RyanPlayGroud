using System.Web;
using AmbientContext.Shared.DotNetStandardLib;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContextDotNetFrameworkWebLib
{
    public class TestHelper
    {
        public static RequestData GetDataFromRequest(HttpRequest request)
        {
            return GetDataFromRequest(new HttpRequestWrapper(request));
        }

        public static RequestData GetDataFromRequest(HttpRequestBase request)
        {
            return new RequestData(
                request.QueryString["userName"],
                long.Parse(request.QueryString["studyId"])
                );
        }

        public static void Verify(HttpContext ctx)
        {
            Verify(new HttpContextWrapper(ctx));
        }

        public static void Verify(HttpContextBase ctx)
        {
            if (!AuthHelper.IsMainRequest(ctx.Request.Url.AbsolutePath)) return;

            var data = TestHelper.GetDataFromRequest(ctx.Request);

            Verifier.VerifyThreadData(data.StudyId);
            // Verifier.VerifyStoreData(data.StudyId); //Can not get study back from store!
        }
    }
}
