using Hackathon.Domain.Enums;

namespace Hackathon.Domain.Entities;

public class AccountApproval
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ReviewedBy { get; set; }
    public UserStatus Status { get; set; }
    public string? Note { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = null!;
    public ApplicationUser? Reviewer { get; set; }
}
