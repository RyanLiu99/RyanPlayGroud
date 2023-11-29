using AmbientContext.Shared.DotNetStandardLib;
using AmbientContext.Shared.DotNetStandardLib.Models;
using Microsoft.AspNetCore.Http;

namespace AmbientContext.AspNetLibInDotNetStandard
{
    public class TestHelper
    {
        public static RequestData GetDataFromRequest(HttpRequest request)
        {
            return new RequestData(
                request.Query["userName"].ToString(),
                long.Parse(request.Query["studyId"][0])
                );
        }

        public static void Verify(HttpContext ctx)
        {
            if (!Verifier.IsVerifyRequired(ctx.Request.Path.Value)) return;

            var data = TestHelper.GetDataFromRequest(ctx.Request);

            Verifier.VerifyAgainstThreadData(ctx.Request.Path.Value, data.StudyId);
        }
    }
}
