using System;
using System.Threading.Tasks;
using AmbientContext.Shared.DotNetStandardLib;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AmbientContext.AspNetLibInDotNetStandard
{

    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _appName;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment, ILogger<AuthMiddleware> logger)
        {
            _next = next;

            _appName = hostEnvironment.ApplicationName;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var data = TestHelper.GetDataFromRequest(context.Request);

            try
            {
                AuthHelper.SetThreadData(data);
                await AsyncActor.DoSthAsync().ConfigureAwait(false);
                Verifier.VerifyAgainstThreadData(context.Request.Path.Value, data.StudyId);
            }
            finally
            {
                await _next(context).ConfigureAwait(false);
            }
            
        }
    }
}
