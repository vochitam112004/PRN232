namespace Hackathon.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MaxTeams { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Event Event { get; set; } = null!;
    public ICollection<CategoryMentor> CategoryMentors { get; set; } = new List<CategoryMentor>();
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}
