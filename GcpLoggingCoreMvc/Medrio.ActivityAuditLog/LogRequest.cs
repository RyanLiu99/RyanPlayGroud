using System;
using System.Collections.Generic;
using System.Text;

namespace Medrio.ActivityAuditLog
{
    public class LogRequest<TPayload>
    {
        public string CustomerId { get; set; }
        public string StudyId { get; set; }
        public TPayload PayLoad { get; set; }
    }
}
