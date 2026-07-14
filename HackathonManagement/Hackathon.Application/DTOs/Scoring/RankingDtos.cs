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

/// <summary>Xếp hạng toàn sự kiện (tổng hợp vòng cuối cùng).</summary>
public class EventRankingResponse
{
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string FinalRoundName { get; set; } = string.Empty;
    public List<TeamResultDto> Results { get; set; } = new();
    public DateTime CalculatedAt { get; set; }
}

/// <summary>Xếp hạng theo hạng mục trong một vòng thi.</summary>
public class CategoryRankingResponse
{
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<TeamResultDto> Results { get; set; } = new();
    public DateTime CalculatedAt { get; set; }
}

/// <summary>Tóm tắt điểm trung bình theo tiêu chí cho một vòng thi.</summary>
public class RoundScoreSummaryResponse
{
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public int TotalSubmissions { get; set; }
    public int TotalJudges { get; set; }
    public List<CriteriaSummaryDto> CriteriaSummaries { get; set; } = new();
}

public class CriteriaSummaryDto
{
    public Guid EventCriteriaId { get; set; }
    public string CriteriaName { get; set; } = string.Empty;
    public decimal MaxScore { get; set; }
    public decimal Weight { get; set; }
    public decimal AverageScore { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScoreGiven { get; set; }
    /// <summary>Độ lệch chuẩn điểm giữa các giám khảo (variance indicator).</summary>
    public decimal StdDeviation { get; set; }
}
