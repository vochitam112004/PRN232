using Hackathon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Application.Interfaces
{
    public interface IAuditLoggerRepository
    {
        Task AddAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<(IEnumerable<AuditLog> Items, int TotalCount)> GetByFilterAsync(
            string? action = null,
            string? targetType = null,
            Guid? performedBy = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 20);
    }
}

