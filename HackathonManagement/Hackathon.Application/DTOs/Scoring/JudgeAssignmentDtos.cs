using System;

namespace Hackathon.Application.DTOs.Scoring;

public class JudgeAssignmentResponse
{
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public Guid JudgeId { get; set; }
    public string JudgeName { get; set; } = string.Empty;
    public string JudgeEmail { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
}

public class AssignJudgeRequest
{
    public Guid JudgeId { get; set; }
}
