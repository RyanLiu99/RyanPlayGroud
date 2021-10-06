using System.Threading.Tasks;
using System.Web;

namespace Medrio.ActivityAuditLog.NetFramework
{
    public interface ILogRequestBuilder<TPayload>
    {
        Task<LogRequest<TPayload>> BuildLogRequest(HttpContext httpContext);

      
    }
}
