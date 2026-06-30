using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IJudgeScoreRepository
{
    Task<IEnumerable<JudgeScore>> GetBySubmissionAsync(Guid submissionId);
    Task<IEnumerable<JudgeScore>> GetByRoundAsync(Guid roundId);
    Task<JudgeScore?> GetScoreAsync(Guid submissionId, Guid judgeId, Guid eventCriteriaId);
    Task AddAsync(JudgeScore score);
    void Update(JudgeScore score);
    Task SaveChangesAsync();
}
