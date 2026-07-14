using Hackathon.Application.DTOs.Auth;

namespace Hackathon.Application.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string[] Errors)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, AuthResponse? Response, string Error)> LoginAsync(LoginRequest request);
    Task<(bool Success, AuthResponse? Response, string Error)> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
}
