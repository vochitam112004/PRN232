using System;

namespace Hackathon.Domain.Entities;

public class JudgeAssignment
{
    public Guid RoundId { get; set; }
    public Guid JudgeId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Round Round { get; set; } = null!;
    public ApplicationUser Judge { get; set; } = null!;
}
