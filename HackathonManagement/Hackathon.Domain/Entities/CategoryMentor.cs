using System;

namespace Hackathon.Domain.Entities;

public class CategoryMentor
{
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid MentorId { get; set; }
    public ApplicationUser Mentor { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
