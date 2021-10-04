using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Medrio.ActivityAuditLog
{
    public interface ILogRequestBuilder<TPayload>
    {
        ValueTask<LogRequest<TPayload>> BuildLogRequest(HttpContext httpContext);

      
    }
}
