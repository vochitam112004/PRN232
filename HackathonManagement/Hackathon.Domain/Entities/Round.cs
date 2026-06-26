using Hackathon.Domain.Enums;

namespace Hackathon.Domain.Entities;

public class Round
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RoundOrder { get; set; }
    public RoundStatus Status { get; set; } = RoundStatus.Upcoming;
    public DateTime? SubmissionStart { get; set; }
    public DateTime SubmissionDeadline { get; set; }
    public DateTime? JudgingStart { get; set; }
    public DateTime? JudgingEnd { get; set; }
    public bool IsCalibrationRound { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Event Event { get; set; } = null!;
    public ICollection<RoundPromotionRule> PromotionRules { get; set; } = new List<RoundPromotionRule>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    
    // Phase 4 properties
    public ICollection<JudgeAssignment> JudgeAssignments { get; set; } = new List<JudgeAssignment>();
    public ICollection<RoundResult> RoundResults { get; set; } = new List<RoundResult>();
}
