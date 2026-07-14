using System;

namespace Hackathon.Application.DTOs.Submission;

public class SubmissionResponse
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;

    public string RepoUrl { get; set; } = string.Empty;
    public string? DemoUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? Description { get; set; }

    // Github/Gitlab Metadata
    public string? RepoDescription { get; set; }
    public int? RepoStars { get; set; }
    public string? RepoLastCommitMessage { get; set; }
    public DateTime? RepoLastCommitDate { get; set; }
    public string? RepoPrimaryLanguage { get; set; }

    public DateTime SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
