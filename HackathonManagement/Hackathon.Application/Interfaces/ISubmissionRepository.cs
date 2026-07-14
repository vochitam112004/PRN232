using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface ISubmissionRepository
{
    Task<Submission?> GetByIdAsync(Guid id);
    Task<Submission?> GetByTeamAndRoundAsync(Guid teamId, Guid roundId);
    Task<IEnumerable<Submission>> GetSubmissionsByTeamAsync(Guid teamId);
    Task<IEnumerable<Submission>> GetSubmissionsByRoundAsync(Guid roundId);
    Task AddAsync(Submission submission);
    void Update(Submission submission);
    Task SaveChangesAsync();
}
