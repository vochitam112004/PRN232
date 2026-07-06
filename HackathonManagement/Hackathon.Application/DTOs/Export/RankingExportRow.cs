namespace Hackathon.Application.DTOs.Export;

public class RankingExportRow
{
    public string RoundName { get; set; } = string.Empty;
    public int Rank { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalScore { get; set; }
    public bool IsAdvanced { get; set; }
    public string? Note { get; set; }
}