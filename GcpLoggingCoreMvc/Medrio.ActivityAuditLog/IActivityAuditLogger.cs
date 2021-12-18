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
    public interface IActivityAuditLogger
    {
        Task<string> WriteLog(HttpContext context);
    }
}
