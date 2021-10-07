
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#if NETFRAMEWORK
using HttpContext = System.Web.HttpContext;
#else
using HttpContext = Microsoft.AspNetCore.Http.HttpContext;
#endif

namespace Medrio.ActivityAuditLog
{
    public abstract class LogRequestBuilder<TPayload> : ILogRequestBuilder<TPayload>
    {
        public const string Header = "header";
        public const string URL = "URL";
        public const string Request = "request";
        public Task<LogRequest<TPayload>> BuildLogRequest(HttpContext httpContext)
        {
            var request = new LogRequest<TPayload>()
            {
                CustomerId = "CId From http context",
                StudyId = "Sid From http context",
                PayLoad = CreatePayLoad(httpContext)
            };

            return Task.FromResult(request);
        }

        protected abstract TPayload CreatePayLoad(HttpContext context);
    }
}
