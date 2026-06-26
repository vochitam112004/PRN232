using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class JudgeAssignmentRepository : IJudgeAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public JudgeAssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JudgeAssignment>> GetByRoundAsync(Guid roundId)
    {
        return await _context.JudgeAssignments
            .Include(j => j.Judge)
            .Where(j => j.RoundId == roundId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JudgeAssignment>> GetByJudgeAsync(Guid judgeId)
    {
        return await _context.JudgeAssignments
            .Include(j => j.Round)
            .Where(j => j.JudgeId == judgeId)
            .ToListAsync();
    }

    public async Task<JudgeAssignment?> GetAsync(Guid roundId, Guid judgeId)
    {
        return await _context.JudgeAssignments
            .FirstOrDefaultAsync(j => j.RoundId == roundId && j.JudgeId == judgeId);
    }

    public async Task AddAsync(JudgeAssignment assignment)
    {
        await _context.JudgeAssignments.AddAsync(assignment);
    }

    public void Remove(JudgeAssignment assignment)
    {
        _context.JudgeAssignments.Remove(assignment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
