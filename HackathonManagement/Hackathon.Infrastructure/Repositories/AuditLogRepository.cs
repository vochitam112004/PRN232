using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
             _db.AuditLogs.AddAsync(log);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _db.AuditLogs.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }
    }
}
