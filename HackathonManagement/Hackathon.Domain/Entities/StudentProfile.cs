namespace Hackathon.Domain.Entities;

public class StudentProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string StudentCode { get; set; } = string.Empty;

    /// <summary>null = FPT student; non-null = external student's university</summary>
    public string? UniversityName { get; set; }
    public bool IsFptStudent { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = null!;
}
