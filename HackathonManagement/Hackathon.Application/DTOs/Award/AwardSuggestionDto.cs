using Hackathon.Domain.Enums;

namespace Hackathon.Application.DTOs.Award;

// 1 dòng gợi ý: hạng này -> đội này -> nên trao giải gì
public class AwardSuggestionDto
{
    public int Rank { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalScore { get; set; }
    public AwardType SuggestedAwardType { get; set; }
    public string SuggestedAwardName { get; set; } = string.Empty;
}
public class GrantAwardRequest
{
    public Guid EventId { get; set; }
    public List<GrantAwardItem> Items { get; set; } = new();
}

public class GrantAwardItem
{
    public Guid TeamId { get; set; }
    public AwardType AwardType { get; set; }
    public string Name { get; set; } = string.Empty;   // vd "Giải Nhất"
    public string? PrizeValue { get; set; }             // vd "10,000,000 VND"
    public Guid? CategoryId { get; set; }               // null = giải toàn sự kiện
    public string? Note { get; set; }
}