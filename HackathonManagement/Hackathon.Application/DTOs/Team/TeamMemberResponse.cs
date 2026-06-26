using System;

namespace Hackathon.Application.DTOs.Team;

public class TeamMemberResponse
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string RoleInTeam { get; set; } = string.Empty; // e.g. "Leader" or "Member"
    public DateTime JoinedAt { get; set; }
}
