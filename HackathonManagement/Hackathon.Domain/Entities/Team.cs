using System;
using System.Collections.Generic;

namespace Hackathon.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string InviteCode { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid LeaderId { get; set; }
    public ApplicationUser Leader { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
