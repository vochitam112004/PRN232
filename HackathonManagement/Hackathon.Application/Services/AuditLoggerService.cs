using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;
using System.Net;
using System.Text.Json;

namespace SEAL.Infrastructure.Services;

public class AuditLoggerService : IAuditLoggerService
{
    private readonly IAuditLoggerRepository _auditLoggerRepository;

    public AuditLoggerService(IAuditLoggerRepository auditLoggerRepository)
    {
        _auditLoggerRepository = auditLoggerRepository;
    }

    public async Task LogAsync(AuditAction action, Guid performedBy, string? targetType = null, Guid? targetId = null, object? payload = null, string? reason = null, string? ipAddress = null)
    {
        var log = new AuditLog
        {
            Action = action,
            PerformedBy = performedBy,
            TargetType = targetType,
            TargetId = targetId,
            Payload = payload is null ? null : JsonSerializer.Serialize(payload),
            Reason = reason,
            IpAddress = ipAddress
            // Id: EF tự sinh. CreatedAt: đã có default trong entity.
        };
        await _auditLoggerRepository.AddAsync(log);
    }
}