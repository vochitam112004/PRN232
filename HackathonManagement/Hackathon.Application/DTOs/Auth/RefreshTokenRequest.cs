using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
