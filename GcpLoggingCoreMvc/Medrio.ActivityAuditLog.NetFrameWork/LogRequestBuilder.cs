using System.Threading.Tasks;
using System.Web;
using Medrio.ActivityAuditLog.NetFramework;

namespace Medrio.ActivityAuditLog.NetFrameWork
{
    public abstract class LogRequestBuilder<TPayload> : ILogRequestBuilder<TPayload>
    {
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


        protected abstract TPayload CreatePayLoad(HttpContext httpContext);

}
}
