using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Medrio.ActivityAuditLog
{
    public abstract class LogRequestBuilder<TPayload> : ILogRequestBuilder<TPayload>
    {
        public ValueTask<LogRequest<TPayload>> BuildLogRequest(HttpContext httpContext)
        {
            var request = new LogRequest<TPayload>()
            {
                CustomerId = "CId From http context",
                StudyId = "Sid From http context",
                PayLoad = CreatePayLoad(httpContext)
            };

            return new ValueTask<LogRequest<TPayload>>(request);
        }


        protected abstract TPayload CreatePayLoad(HttpContext httpContext);

}
}
