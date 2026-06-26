using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly ApplicationDbContext _db;

    public SubmissionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Submission?> GetByIdAsync(Guid id)
    {
        return await _db.Submissions
            .Include(s => s.Team)
            .Include(s => s.Round)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Submission?> GetByTeamAndRoundAsync(Guid teamId, Guid roundId)
    {
        return await _db.Submissions
            .Include(s => s.Team)
            .Include(s => s.Round)
            .FirstOrDefaultAsync(s => s.TeamId == teamId && s.RoundId == roundId);
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByTeamAsync(Guid teamId)
    {
        return await _db.Submissions
            .Include(s => s.Team)
            .Include(s => s.Round)
            .Where(s => s.TeamId == teamId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByRoundAsync(Guid roundId)
    {
        return await _db.Submissions
            .Include(s => s.Team)
            .Include(s => s.Round)
            .Where(s => s.RoundId == roundId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Submission submission)
    {
        await _db.Submissions.AddAsync(submission);
    }

    public void Update(Submission submission)
    {
        _db.Submissions.Update(submission);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
