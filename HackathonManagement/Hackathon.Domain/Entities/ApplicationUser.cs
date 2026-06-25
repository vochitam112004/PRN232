using Hackathon.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Hackathon.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public UserStatus Status { get; set; } = UserStatus.PendingApproval;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public StudentProfile? StudentProfile { get; set; }
    public ICollection<AccountApproval> AccountApprovals { get; set; } = new List<AccountApproval>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<CriteriaTemplate> CreatedTemplates { get; set; } = new List<CriteriaTemplate>();
    public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
}

