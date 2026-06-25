using Hackathon.Domain.Enums;

namespace Hackathon.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BannerUrl { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public DateTime? RegistrationStart { get; set; }
    public DateTime? RegistrationEnd { get; set; }
    public Guid? CriteriaTemplateId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ApplicationUser Creator { get; set; } = null!;
    public CriteriaTemplate? CriteriaTemplate { get; set; }
    public ICollection<EventCriteria> Criteria { get; set; } = new List<EventCriteria>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}
