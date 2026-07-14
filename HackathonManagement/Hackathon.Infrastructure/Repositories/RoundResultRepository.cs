using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class RoundResultRepository : IRoundResultRepository
{
    private readonly ApplicationDbContext _context;

    public RoundResultRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoundResult>> GetByRoundAsync(Guid roundId)
    {
        return await _context.RoundResults
            .Include(r => r.Submission)
                .ThenInclude(s => s.Team)
            .Where(r => r.RoundId == roundId)
            .OrderBy(r => r.Rank)
            .ToListAsync();
    }

    public async Task<RoundResult?> GetBySubmissionAsync(Guid submissionId, Guid roundId)
    {
        return await _context.RoundResults
            .FirstOrDefaultAsync(r => r.SubmissionId == submissionId && r.RoundId == roundId);
    }

    public async Task AddRangeAsync(IEnumerable<RoundResult> results)
    {
        await _context.RoundResults.AddRangeAsync(results);
    }

    public void RemoveRange(IEnumerable<RoundResult> results)
    {
        _context.RoundResults.RemoveRange(results);
    }

    public void UpdateRange(IEnumerable<RoundResult> results)
    {
        _context.RoundResults.UpdateRange(results);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
