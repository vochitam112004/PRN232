using System;

namespace Hackathon.Domain.Entities;

public class RoundResult
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public Guid SubmissionId { get; set; }
    public decimal TotalScore { get; set; }
    public int Rank { get; set; }
    public bool IsAdvanced { get; set; }
    public string? Note { get; set; }
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Round Round { get; set; } = null!;
    public Submission Submission { get; set; } = null!;
}
