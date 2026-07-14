using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class JudgeScoreRepository : IJudgeScoreRepository
{
    private readonly ApplicationDbContext _context;

    public JudgeScoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JudgeScore>> GetBySubmissionAsync(Guid submissionId)
    {
        return await _context.JudgeScores
            .Include(s => s.Judge)
            .Include(s => s.EventCriteria)
            .Where(s => s.SubmissionId == submissionId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JudgeScore>> GetByRoundAsync(Guid roundId)
    {
        return await _context.JudgeScores
            .Include(s => s.Submission)
            .Where(s => s.Submission.RoundId == roundId)
            .ToListAsync();
    }

    public async Task<JudgeScore?> GetScoreAsync(Guid submissionId, Guid judgeId, Guid eventCriteriaId)
    {
        return await _context.JudgeScores
            .FirstOrDefaultAsync(s => s.SubmissionId == submissionId 
                                   && s.JudgeId == judgeId 
                                   && s.EventCriteriaId == eventCriteriaId);
    }

    public async Task AddAsync(JudgeScore score)
    {
        await _context.JudgeScores.AddAsync(score);
    }

    public void Update(JudgeScore score)
    {
        _context.JudgeScores.Update(score);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
