using System.Threading.Tasks;
using System.Web;

namespace Medrio.ActivityAuditLog.NetFramework
{
    public interface IActivityAuditLogger
    {
        Task<string> WriteLog(HttpContext context);
    }
}
