namespace Hackathon.Domain.Entities;

public class AwardRecipient
{
    public Guid Id { get; set; }
    public Guid AwardId { get; set; }
    public Award Award { get; set; } = null!;

    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public Guid GrantedBy { get; set; }                 // BTC trao giải (ApplicationUser)
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}