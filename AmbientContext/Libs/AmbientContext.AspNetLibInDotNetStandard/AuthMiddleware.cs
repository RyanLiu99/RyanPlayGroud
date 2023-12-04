using System;
using System.Threading.Tasks;
using AmbientContext.Shared.DotNetStandardLib;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace AmbientContext.AspNetCoreLibInDotNetStandard
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!AuthHelper.IsMainRequest(context.Request.Path.Value))
                    return;

                var data = TestHelper.GetDataFromRequest(context.Request);

                AuthHelper.SetThreadData(data);
                await AsyncActor.DoSthAsync().ConfigureAwait(false);

                Verifier.VerifyThreadData(data.StudyId);
                Verifier.VerifyStoreData(data.StudyId);

                await _next(context).ConfigureAwait(false);

                var notVerifyAtEnd = context.Request.Query.ContainsKey("notVerifyAtEndRequest");

                if (!notVerifyAtEnd) // studyId manually changed, not long as the one in query string
                {
                    Verifier.VerifyThreadData(data.StudyId);
                    Verifier.VerifyStoreData(data.StudyId);
                }
            }
            catch (Exception ex) 
            {
                throw;// for debug purpose
            }
        }
    }
}
