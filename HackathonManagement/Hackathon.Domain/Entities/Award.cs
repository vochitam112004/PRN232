using Hackathon.Domain.Enums;

namespace Hackathon.Domain.Entities;

public class Award
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;

    public string Name { get; set; } = string.Empty;   // vd "Giải Nhất"
    public AwardType AwardType { get; set; }
    public string? Description { get; set; }
    public string? PrizeValue { get; set; }             // vd "10,000,000 VND"

    public Guid? CategoryId { get; set; }               // null = giải toàn sự kiện
    public Category? Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AwardRecipient> Recipients { get; set; } = new List<AwardRecipient>();

}