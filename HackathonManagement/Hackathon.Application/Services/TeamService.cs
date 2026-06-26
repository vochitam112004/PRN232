using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Team;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepo;
    private readonly IUserRepository _userRepo;
    private readonly IEventRepository _eventRepo;
    private readonly IStudentProfileRepository _studentProfileRepo;

    public TeamService(
        ITeamRepository teamRepo,
        IUserRepository userRepo,
        IEventRepository eventRepo,
        IStudentProfileRepository studentProfileRepo)
    {
        _teamRepo = teamRepo;
        _userRepo = userRepo;
        _eventRepo = eventRepo;
        _studentProfileRepo = studentProfileRepo;
    }

    public async Task<TeamResponse> CreateTeamAsync(Guid leaderId, CreateTeamRequest request)
    {
        var leader = await _userRepo.FindByIdAsync(leaderId);
        if (leader == null)
            throw new ArgumentException("Không tìm thấy người dùng.");

        if (leader.Status != UserStatus.Approved)
            throw new InvalidOperationException("Tài khoản của bạn chưa được phê duyệt hoặc đang bị khóa.");

        // Check if student profile exists
        var profile = await _studentProfileRepo.FindByUserIdAsync(leaderId);
        if (profile == null)
            throw new InvalidOperationException("Bạn cần bổ sung thông tin sinh viên trước khi tạo đội.");

        // Retrieve event of category
        var ev = await _eventRepo.GetByIdWithDetailsAsync(request.CategoryId); // wait, eventRepo takes eventId, but we need event of category
        // Let's get category first. We can load event via category, but since eventRepo doesn't load categories directly by categoryId, let's load all events and find category, or we can queries DB.
        // Wait, how can we load Category? Let's check Category loading in EventRepository.
        // It's easier if we query the DbContext or load event details.
        // Let's load all events and find category, or better:
        var allEvents = await _eventRepo.GetAllAsync();
        Category? category = null;
        Event? parentEvent = null;
        foreach (var e in allEvents)
        {
            var cat = e.Categories.FirstOrDefault(c => c.Id == request.CategoryId);
            if (cat != null)
            {
                category = cat;
                parentEvent = e;
                break;
            }
        }

        if (category == null || parentEvent == null)
            throw new ArgumentException("Hạng mục thi đấu không tồn tại.");

        // Check if registration period is active
        var now = DateTime.UtcNow;
        if (parentEvent.RegistrationStart.HasValue && now < parentEvent.RegistrationStart.Value)
            throw new InvalidOperationException($"Thời gian đăng ký sự kiện chưa bắt đầu (Bắt đầu lúc: {parentEvent.RegistrationStart.Value.ToLocalTime()}).");
        if (parentEvent.RegistrationEnd.HasValue && now > parentEvent.RegistrationEnd.Value)
            throw new InvalidOperationException("Thời gian đăng ký sự kiện đã kết thúc.");

        // Verify leader is not already in any team in this event
        var existingTeam = await _teamRepo.GetByStudentIdInEventAsync(leaderId, parentEvent.Id);
        if (existingTeam != null)
            throw new InvalidOperationException("Bạn đã tham gia hoặc làm trưởng nhóm của một đội thi khác trong sự kiện này.");

        // Check category max teams limit
        if (category.MaxTeams.HasValue)
        {
            var categoryTeams = await _teamRepo.GetTeamsByCategoryAsync(category.Id);
            if (categoryTeams.Count() >= category.MaxTeams.Value)
                throw new InvalidOperationException($"Hạng mục này đã đạt số lượng đội đăng ký tối đa ({category.MaxTeams.Value} đội).");
        }

        // Generate unique InviteCode
        string inviteCode = string.Empty;
        bool unique = false;
        while (!unique)
        {
            inviteCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            var checkTeam = await _teamRepo.GetByInviteCodeAsync(inviteCode);
            if (checkTeam == null) unique = true;
        }

        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            InviteCode = inviteCode,
            CategoryId = request.CategoryId,
            LeaderId = leaderId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add leader as a member
        team.Members.Add(new TeamMember
        {
            TeamId = team.Id,
            UserId = leaderId,
            JoinedAt = DateTime.UtcNow
        });

        await _teamRepo.AddAsync(team);
        await _teamRepo.SaveChangesAsync();

        // Load details for mapping
        var savedTeam = await _teamRepo.GetByIdWithMembersAsync(team.Id);
        return MapToTeamResponse(savedTeam!);
    }

    public async Task<TeamResponse> JoinTeamAsync(Guid userId, JoinTeamRequest request)
    {
        var user = await _userRepo.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("Không tìm thấy người dùng.");

        if (user.Status != UserStatus.Approved)
            throw new InvalidOperationException("Tài khoản của bạn chưa được phê duyệt hoặc đang bị khóa.");

        var profile = await _studentProfileRepo.FindByUserIdAsync(userId);
        if (profile == null)
            throw new InvalidOperationException("Bạn cần bổ sung thông tin sinh viên trước khi tham gia đội.");

        var team = await _teamRepo.GetByInviteCodeAsync(request.InviteCode);
        if (team == null)
            throw new ArgumentException("Mã mời không chính xác hoặc đội thi không tồn tại.");

        // Check if registration period is active
        var now = DateTime.UtcNow;
        var parentEvent = team.Category.Event;
        if (parentEvent.RegistrationStart.HasValue && now < parentEvent.RegistrationStart.Value)
            throw new InvalidOperationException($"Thời gian đăng ký sự kiện chưa bắt đầu (Bắt đầu lúc: {parentEvent.RegistrationStart.Value.ToLocalTime()}).");
        if (parentEvent.RegistrationEnd.HasValue && now > parentEvent.RegistrationEnd.Value)
            throw new InvalidOperationException("Thời gian đăng ký sự kiện đã kết thúc.");

        // Verify user is not already in any team in this event
        var existingTeam = await _teamRepo.GetByStudentIdInEventAsync(userId, parentEvent.Id);
        if (existingTeam != null)
            throw new InvalidOperationException("Bạn đã tham gia hoặc làm trưởng nhóm của một đội thi khác trong sự kiện này.");

        // Check team size (limit 3-5 members)
        if (team.Members.Count >= 5)
            throw new InvalidOperationException("Đội thi đã đạt số lượng thành viên tối đa (5 thành viên).");

        team.Members.Add(new TeamMember
        {
            TeamId = team.Id,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        });

        team.UpdatedAt = DateTime.UtcNow;
        await _teamRepo.SaveChangesAsync();

        return MapToTeamResponse(team);
    }

    public async Task<bool> LeaveTeamAsync(Guid userId, Guid teamId)
    {
        var team = await _teamRepo.GetByIdWithMembersAsync(teamId);
        if (team == null)
            throw new ArgumentException("Không tìm thấy đội thi.");

        var member = team.Members.FirstOrDefault(m => m.UserId == userId);
        if (member == null)
            return false;

        // Check registration period
        var now = DateTime.UtcNow;
        var parentEvent = team.Category.Event;
        if (parentEvent.RegistrationEnd.HasValue && now > parentEvent.RegistrationEnd.Value)
            throw new InvalidOperationException("Thời gian đăng ký sự kiện đã kết thúc, bạn không thể rời đội thi lúc này.");

        if (team.LeaderId == userId)
        {
            // If the leader is the only member, disband the team
            if (team.Members.Count == 1)
            {
                _teamRepo.Delete(team);
            }
            else
            {
                throw new InvalidOperationException("Trưởng nhóm không thể rời đội. Vui lòng chuyển quyền trưởng nhóm hoặc giải tán đội thi.");
            }
        }
        else
        {
            team.Members.Remove(member);
            team.UpdatedAt = DateTime.UtcNow;
        }

        await _teamRepo.SaveChangesAsync();
        return true;
    }

    public async Task<TeamResponse?> GetTeamByIdAsync(Guid teamId)
    {
        var team = await _teamRepo.GetByIdWithMembersAsync(teamId);
        if (team == null) return null;
        return MapToTeamResponse(team);
    }

    public async Task<TeamResponse?> GetStudentTeamAsync(Guid studentId, Guid eventId)
    {
        var team = await _teamRepo.GetByStudentIdInEventAsync(studentId, eventId);
        if (team == null) return null;
        
        // Re-load with full details
        return await GetTeamByIdAsync(team.Id);
    }

    public async Task<IEnumerable<TeamResponse>> GetTeamsByCategoryAsync(Guid categoryId)
    {
        var teams = await _teamRepo.GetTeamsByCategoryAsync(categoryId);
        return teams.Select(MapToTeamResponse);
    }

    public async Task<(bool Success, string Error)> AssignMentorToCategoryAsync(Guid categoryId, Guid mentorId)
    {
        var allEvents = await _eventRepo.GetAllAsync();
        Category? category = null;
        foreach (var e in allEvents)
        {
            var cat = e.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (cat != null)
            {
                category = cat;
                break;
            }
        }

        if (category == null)
            return (false, "Không tìm thấy hạng mục thi đấu.");

        var mentor = await _userRepo.FindByIdAsync(mentorId);
        if (mentor == null)
            return (false, "Không tìm thấy người dùng.");

        var roles = await _userRepo.GetRolesAsync(mentor);
        if (!roles.Contains(Roles.Mentor))
            return (false, "Người dùng này không có quyền Mentor.");

        var existingAssignment = await _teamRepo.GetCategoryMentorAsync(categoryId, mentorId);
        if (existingAssignment != null)
            return (false, "Mentor đã được phân công cho hạng mục này rồi.");

        var assignment = new CategoryMentor
        {
            CategoryId = categoryId,
            MentorId = mentorId,
            AssignedAt = DateTime.UtcNow
        };

        await _teamRepo.AddCategoryMentorAsync(assignment);
        await _teamRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error)> RemoveMentorFromCategoryAsync(Guid categoryId, Guid mentorId)
    {
        var assignment = await _teamRepo.GetCategoryMentorAsync(categoryId, mentorId);
        if (assignment == null)
            return (false, "Không tìm thấy thông tin phân công.");

        _teamRepo.RemoveCategoryMentor(assignment);
        await _teamRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<IEnumerable<TeamMemberResponse>> GetCategoryMentorsAsync(Guid categoryId)
    {
        var assignments = await _teamRepo.GetCategoryMentorsByCategoryIdAsync(categoryId);
        return assignments.Select(a => new TeamMemberResponse
        {
            UserId = a.MentorId,
            FullName = a.Mentor.FullName,
            Email = a.Mentor.Email ?? string.Empty,
            AvatarUrl = a.Mentor.AvatarUrl,
            RoleInTeam = "Mentor",
            JoinedAt = a.AssignedAt
        });
    }

    private static TeamResponse MapToTeamResponse(Team team)
    {
        return new TeamResponse
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            InviteCode = team.InviteCode,
            CategoryId = team.CategoryId,
            CategoryName = team.Category.Name,
            EventId = team.Category.EventId,
            EventTitle = team.Category.Event?.Title ?? string.Empty,
            LeaderId = team.LeaderId,
            LeaderName = team.Leader.FullName,
            CreatedAt = team.CreatedAt,
            UpdatedAt = team.UpdatedAt,
            Members = team.Members.Select(m => new TeamMemberResponse
            {
                UserId = m.UserId,
                FullName = m.User.FullName,
                Email = m.User.Email ?? string.Empty,
                AvatarUrl = m.User.AvatarUrl,
                RoleInTeam = m.UserId == team.LeaderId ? "Leader" : "Member",
                JoinedAt = m.JoinedAt
            }).ToList()
        };
    }
}
