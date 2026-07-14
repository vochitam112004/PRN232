using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Auth;

public class CreateGuestJudgeRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    /// <summary>Temporary password — should be sent via email</summary>
    [Required, MinLength(6)]
    public string TemporaryPassword { get; set; } = string.Empty;
}
