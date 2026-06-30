using System;
using System.Collections.Generic;

namespace Hackathon.Application.DTOs.Scoring;

public class RoundRankingResponse
{
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public List<TeamResultDto> Results { get; set; } = new();
    public DateTime CalculatedAt { get; set; }
}

public class TeamResultDto
{
    public Guid SubmissionId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalScore { get; set; }
    public int Rank { get; set; }
    public bool IsAdvanced { get; set; }
    public string? Note { get; set; }
}
