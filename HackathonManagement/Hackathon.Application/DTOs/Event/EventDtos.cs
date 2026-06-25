using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Event;

public class CreateEventRequest
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BannerUrl { get; set; }
    public DateTime? RegistrationStart { get; set; }
    public DateTime? RegistrationEnd { get; set; }
    public Guid? CriteriaTemplateId { get; set; }
}

public class EventResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BannerUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? RegistrationStart { get; set; }
    public DateTime? RegistrationEnd { get; set; }
    public Guid? CriteriaTemplateId { get; set; }
    public Guid CreatedBy { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<EventCriteriaResponse> Criteria { get; set; } = new();
    public List<CategoryResponse> Categories { get; set; } = new();
    public List<RoundResponse> Rounds { get; set; } = new();
}

public class EventCriteriaResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Weight { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public Guid? SourceItemId { get; set; }
}

public class EventCriteriaDto
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Range(0.01, 100.0)]
    public decimal MaxScore { get; set; }

    [Range(0.01, 1.0)]
    public decimal Weight { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateEventCriteriaRequest
{
    [Required]
    public List<EventCriteriaDto> Criteria { get; set; } = new();
}

public class CreateCategoryRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MaxTeams { get; set; }
}

public class CategoryResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MaxTeams { get; set; }
}

public class ConfigureRoundRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    [Required]
    public int RoundOrder { get; set; }
    
    public DateTime? SubmissionStart { get; set; }
    
    [Required]
    public DateTime SubmissionDeadline { get; set; }
    
    public DateTime? JudgingStart { get; set; }
    public DateTime? JudgingEnd { get; set; }
    public bool IsCalibrationRound { get; set; }
    
    public List<PromotionRuleDto> PromotionRules { get; set; } = new();
}

public class PromotionRuleDto
{
    [Required]
    public string RuleType { get; set; } = string.Empty; // top_n_per_category | top_n_overall | score_threshold
    public int? TopN { get; set; }
    public decimal? ScoreThreshold { get; set; }
    public bool PerCategory { get; set; } = true;
}

public class RoundResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RoundOrder { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? SubmissionStart { get; set; }
    public DateTime SubmissionDeadline { get; set; }
    public DateTime? JudgingStart { get; set; }
    public DateTime? JudgingEnd { get; set; }
    public bool IsCalibrationRound { get; set; }
    public List<PromotionRuleResponse> PromotionRules { get; set; } = new();
}

public class PromotionRuleResponse
{
    public Guid Id { get; set; }
    public string RuleType { get; set; } = string.Empty;
    public int? TopN { get; set; }
    public decimal? ScoreThreshold { get; set; }
    public bool PerCategory { get; set; }
}
