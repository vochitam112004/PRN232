using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Auth;

public class ApprovalActionRequest
{
    [Required]
    public Guid UserId { get; set; }

    /// <summary>true = approve; false = reject</summary>
    [Required]
    public bool Approve { get; set; }

    public string? Note { get; set; }
}
