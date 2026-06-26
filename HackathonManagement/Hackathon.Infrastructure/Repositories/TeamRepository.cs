using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _db;

    public TeamRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Team?> GetByIdAsync(Guid id)
    {
        return await _db.Teams
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Team?> GetByIdWithMembersAsync(Guid id)
    {
        return await _db.Teams
            .Include(t => t.Category)
                .ThenInclude(c => c.Event)
            .Include(t => t.Leader)
            .Include(t => t.Members)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Team?> GetByInviteCodeAsync(string inviteCode)
    {
        return await _db.Teams
            .Include(t => t.Category)
                .ThenInclude(c => c.Event)
            .Include(t => t.Leader)
            .Include(t => t.Members)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.InviteCode == inviteCode.Trim());
    }

    public async Task<Team?> GetByStudentIdInEventAsync(Guid studentId, Guid eventId)
    {
        // Find team where user is member or leader, and team belongs to this event
        return await _db.Teams
            .Include(t => t.Category)
            .Include(t => t.Leader)
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => 
                t.Category.EventId == eventId && 
                (t.LeaderId == studentId || t.Members.Any(m => m.UserId == studentId))
            );
    }

    public async Task<IEnumerable<Team>> GetTeamsByCategoryAsync(Guid categoryId)
    {
        return await _db.Teams
            .Include(t => t.Category)
            .Include(t => t.Leader)
            .Include(t => t.Members)
                .ThenInclude(m => m.User)
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task AddAsync(Team team)
    {
        await _db.Teams.AddAsync(team);
    }

    public void Update(Team team)
    {
        _db.Teams.Update(team);
    }

    public void Delete(Team team)
    {
        _db.Teams.Remove(team);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    // CategoryMentor assignments
    public async Task AddCategoryMentorAsync(CategoryMentor cm)
    {
        await _db.CategoryMentors.AddAsync(cm);
    }

    public void RemoveCategoryMentor(CategoryMentor cm)
    {
        _db.CategoryMentors.Remove(cm);
    }

    public async Task<CategoryMentor?> GetCategoryMentorAsync(Guid categoryId, Guid mentorId)
    {
        return await _db.CategoryMentors
            .FirstOrDefaultAsync(cm => cm.CategoryId == categoryId && cm.MentorId == mentorId);
    }

    public async Task<IEnumerable<CategoryMentor>> GetCategoryMentorsByCategoryIdAsync(Guid categoryId)
    {
        return await _db.CategoryMentors
            .Include(cm => cm.Mentor)
            .Where(cm => cm.CategoryId == categoryId)
            .ToListAsync();
    }
}
