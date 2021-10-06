namespace Medrio.ActivityAuditLog.NetFramework
{
    public class LogRequest<TPayload>
    {
        public string CustomerId { get; set; }
        public string StudyId { get; set; }
        public TPayload PayLoad { get; set; }
    }
}
