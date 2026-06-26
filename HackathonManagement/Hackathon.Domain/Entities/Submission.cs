using System;

namespace Hackathon.Domain.Entities;

public class Submission
{
    public Guid Id { get; set; }
    
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public Guid RoundId { get; set; }
    public Round Round { get; set; } = null!;

    // Links for submissions (as multiple URLs can be submitted)
    public string RepoUrl { get; set; } = string.Empty; // GitHub/GitLab URL
    public string? DemoUrl { get; set; } // Deployed live site URL
    public string? VideoUrl { get; set; } // YouTube demo video URL
    public string? Description { get; set; } // General notes/description

    // Github/Gitlab metadata details retrieved via API
    public string? RepoDescription { get; set; }
    public int? RepoStars { get; set; }
    public string? RepoLastCommitMessage { get; set; }
    public DateTime? RepoLastCommitDate { get; set; }
    public string? RepoPrimaryLanguage { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
