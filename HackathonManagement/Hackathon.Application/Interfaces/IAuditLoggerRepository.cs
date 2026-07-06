using Hackathon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Application.Interfaces
{
    public interface IAuditLoggerRepository
    {
        Task AddAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetAllAsync();
    }
}
