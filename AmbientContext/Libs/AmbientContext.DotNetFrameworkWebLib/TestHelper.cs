using System.Web;
using AmbientContext.Shared.DotNetStandardLib;
using AmbientContext.Shared.DotNetStandardLib.Models;

namespace AmbientContextDotNetFrameworkWebLib
{
    public class TestHelper
    {
        public static RequestData GetDataFromRequest(HttpRequest request)
        {
            return new RequestData(
                request.QueryString["userName"],
                long.Parse(request.QueryString["studyId"])
                );
        }

        public static void Verify(HttpContext ctx)
        {
            if (!AuthHelper.IsMainRequest(ctx.Request.Url.AbsolutePath)) return;

            var data = TestHelper.GetDataFromRequest(ctx.Request);

            Verifier.VerifyThreadData(data.StudyId);
        }
    }
}
