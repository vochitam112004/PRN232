using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.CriteriaTemplate;

public class CreateCriteriaTemplateRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public List<CreateCriteriaTemplateItemDto> Items { get; set; } = new();
}

public class CreateCriteriaTemplateItemDto
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    [Range(0.01, 100.0)]
    public decimal MaxScore { get; set; }
    
    [Range(0.01, 1.0)]
    public decimal Weight { get; set; }
    public int DisplayOrder { get; set; }
}

public class CriteriaTemplateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CriteriaTemplateItemResponse> Items { get; set; } = new();
}

public class CriteriaTemplateItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Weight { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateCriteriaTemplateRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
}

public class UpdateCriteriaTemplateItemsRequest
{
    [Required]
    public List<CreateCriteriaTemplateItemDto> Items { get; set; } = new();
}
