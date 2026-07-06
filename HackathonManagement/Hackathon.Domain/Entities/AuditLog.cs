using Hackathon.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }                       // KHÔNG gán — để EF tự sinh (giống Event)
        public AuditAction Action { get; set; }
        public Guid PerformedBy { get; set; }
        public string? TargetType { get; set; }
        public Guid? TargetId { get; set; }
        public string? Payload { get; set; }
        public string? Reason { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;   // giống Event

        // Navigation — khớp convention 'Creator' của Event (ApplicationUser cùng namespace nên không cần using)
        public ApplicationUser PerformedByUser { get; set; } = null!;
    }
}
