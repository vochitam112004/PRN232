namespace Hackathon.Domain.Entities;

public class CriteriaTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; } = false;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ApplicationUser Creator { get; set; } = null!;
    public ICollection<CriteriaTemplateItem> Items { get; set; } = new List<CriteriaTemplateItem>();
}
