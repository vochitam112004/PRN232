using Hackathon.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Application.Interfaces
{
    public interface IAuditLoggerService
    {
        Task LogAsync(
            AuditAction action,
            Guid performedBy,
            string? targetType = null,
            Guid? targetId = null,
            object? payload = null,
            string? reason = null,
            string? ipAddress = null);
    }
}
