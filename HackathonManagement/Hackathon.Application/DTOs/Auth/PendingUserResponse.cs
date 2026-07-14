namespace Hackathon.Application.DTOs.Auth;

public class PendingUserResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsFptStudent { get; set; }
    public string StudentCode { get; set; } = string.Empty;
    public string? UniversityName { get; set; }
    public DateTime RegisteredAt { get; set; }
}
