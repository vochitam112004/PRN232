namespace Hackathon.Domain.Entities;

public class CriteriaTemplateItem
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Weight { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public CriteriaTemplate Template { get; set; } = null!;
}
