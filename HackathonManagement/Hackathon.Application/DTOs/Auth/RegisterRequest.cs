using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required]
    public string StudentCode { get; set; } = string.Empty;

    /// <summary>true = FPT student; false = external student (UniversityName required)</summary>
    [Required]
    public bool IsFptStudent { get; set; }

    /// <summary>Required when IsFptStudent = false</summary>
    public string? UniversityName { get; set; }
}
