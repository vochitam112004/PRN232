using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Team;

namespace Hackathon.Application.Interfaces;

public interface ITeamService
{
    Task<TeamResponse> CreateTeamAsync(Guid leaderId, CreateTeamRequest request);
    Task<TeamResponse> JoinTeamAsync(Guid userId, JoinTeamRequest request);
    Task<bool> LeaveTeamAsync(Guid userId, Guid teamId);
    Task<TeamResponse?> GetTeamByIdAsync(Guid teamId);
    Task<TeamResponse?> GetStudentTeamAsync(Guid studentId, Guid eventId);
    Task<IEnumerable<TeamResponse>> GetTeamsByCategoryAsync(Guid categoryId);
    
    // Mentor assignments
    Task<(bool Success, string Error)> AssignMentorToCategoryAsync(Guid categoryId, Guid mentorId);
    Task<(bool Success, string Error)> RemoveMentorFromCategoryAsync(Guid categoryId, Guid mentorId);
    Task<IEnumerable<TeamMemberResponse>> GetCategoryMentorsAsync(Guid categoryId);
}
