using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLoggerRepository
    {
        private readonly ApplicationDbContext _db;

        public AuditLogRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(AuditLog log)
        {
            await _db.AuditLogs.AddAsync(log);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _db.AuditLogs
                .Include(l => l.PerformedByUser)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<(IEnumerable<AuditLog> Items, int TotalCount)> GetByFilterAsync(
            string? action = null,
            string? targetType = null,
            Guid? performedBy = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _db.AuditLogs
                .Include(l => l.PerformedByUser)
                .AsQueryable();

            // Filter by action name (case-insensitive)
            if (!string.IsNullOrWhiteSpace(action))
            {
                var lowerAction = action.Trim().ToLower();
                query = query.Where(l => l.Action.ToString().ToLower().Contains(lowerAction));
            }

            if (!string.IsNullOrWhiteSpace(targetType))
                query = query.Where(l => l.TargetType != null && l.TargetType.ToLower() == targetType.Trim().ToLower());

            if (performedBy.HasValue)
                query = query.Where(l => l.PerformedBy == performedBy.Value);

            if (from.HasValue)
                query = query.Where(l => l.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(l => l.CreatedAt <= to.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
