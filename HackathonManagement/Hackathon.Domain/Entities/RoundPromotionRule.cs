using Hackathon.Domain.Enums;

namespace Hackathon.Domain.Entities;

public class RoundPromotionRule
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public PromotionRuleType RuleType { get; set; }
    public int? TopN { get; set; }
    public decimal? ScoreThreshold { get; set; }
    public bool PerCategory { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Round Round { get; set; } = null!;
}
