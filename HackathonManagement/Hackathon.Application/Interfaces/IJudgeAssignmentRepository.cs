using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IJudgeAssignmentRepository
{
    Task<IEnumerable<JudgeAssignment>> GetByRoundAsync(Guid roundId);
    Task<IEnumerable<JudgeAssignment>> GetByJudgeAsync(Guid judgeId);
    Task<JudgeAssignment?> GetAsync(Guid roundId, Guid judgeId);
    Task AddAsync(JudgeAssignment assignment);
    void Remove(JudgeAssignment assignment);
    Task SaveChangesAsync();
}
