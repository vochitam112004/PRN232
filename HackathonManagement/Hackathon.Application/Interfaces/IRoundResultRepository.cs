using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IRoundResultRepository
{
    Task<IEnumerable<RoundResult>> GetByRoundAsync(Guid roundId);
    Task<RoundResult?> GetBySubmissionAsync(Guid submissionId, Guid roundId);
    Task AddRangeAsync(IEnumerable<RoundResult> results);
    void RemoveRange(IEnumerable<RoundResult> results);
    void UpdateRange(IEnumerable<RoundResult> results);
    Task SaveChangesAsync();
}
