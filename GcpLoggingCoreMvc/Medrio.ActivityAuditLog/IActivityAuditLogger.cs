using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Medrio.ActivityAuditLog
{
    public interface IActivityAuditLogger
    {
        Task<string> WriteLog(HttpContext context);
    }
}
