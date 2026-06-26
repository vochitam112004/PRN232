using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<Team?> GetByIdWithMembersAsync(Guid id);
    Task<Team?> GetByInviteCodeAsync(string inviteCode);
    Task<Team?> GetByStudentIdInEventAsync(Guid studentId, Guid eventId);
    Task<IEnumerable<Team>> GetTeamsByCategoryAsync(Guid categoryId);
    Task AddAsync(Team team);
    void Update(Team team);
    void Delete(Team team);
    Task SaveChangesAsync();

    // CategoryMentor assignments
    Task AddCategoryMentorAsync(CategoryMentor cm);
    void RemoveCategoryMentor(CategoryMentor cm);
    Task<CategoryMentor?> GetCategoryMentorAsync(Guid categoryId, Guid mentorId);
    Task<IEnumerable<CategoryMentor>> GetCategoryMentorsByCategoryIdAsync(Guid categoryId);
}
