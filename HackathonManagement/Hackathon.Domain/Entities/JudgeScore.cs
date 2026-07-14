using System;

namespace Hackathon.Domain.Entities;

public class JudgeScore
{
    public Guid Id { get; set; }
    public Guid SubmissionId { get; set; }
    public Guid JudgeId { get; set; }
    public Guid EventCriteriaId { get; set; }
    public decimal Score { get; set; }
    public string? Comment { get; set; }
    public DateTime ScoredAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Submission Submission { get; set; } = null!;
    public ApplicationUser Judge { get; set; } = null!;
    public EventCriteria EventCriteria { get; set; } = null!;
}
