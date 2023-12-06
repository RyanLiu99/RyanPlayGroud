using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmbientContext.Shared.DotNetStandardLib;
using AmbientContext.Shared.DotNetStandardLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

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
                var request = context.Request;
                if (!AuthHelper.IsMainRequest(request.Path.Value))
                    return;
                RequestData data;
                try
                {
                    data = TestHelper.GetDataFromQueryString(request);
                }
                catch (Exception e)
                {
                    var query = "userName=Ryan&studyId=130";
                    context.Response.Redirect($"{request.PathBase}{request.Path}?{query}");
                    return;
                }
                
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
