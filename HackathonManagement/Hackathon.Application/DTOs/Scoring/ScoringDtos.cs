using System;
using System.Collections.Generic;

namespace Hackathon.Application.DTOs.Scoring;

public class CriteriaScoreDto
{
    public Guid EventCriteriaId { get; set; }
    public decimal Score { get; set; }
    public string? Comment { get; set; }
}

public class SubmitScoreRequest
{
    public List<CriteriaScoreDto> Scores { get; set; } = new();
}

public class SubmissionScoreResponse
{
    public Guid SubmissionId { get; set; }
    public Guid JudgeId { get; set; }
    public string JudgeName { get; set; } = string.Empty;
    public List<CriteriaScoreResponse> Scores { get; set; } = new();
    public decimal TotalScore { get; set; }
    public DateTime ScoredAt { get; set; }
}

public class CriteriaScoreResponse
{
    public Guid EventCriteriaId { get; set; }
    public string CriteriaName { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Weight { get; set; }
    public string? Comment { get; set; }
}
