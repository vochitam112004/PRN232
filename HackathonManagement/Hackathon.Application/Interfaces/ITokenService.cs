using System.Security.Claims;

namespace Hackathon.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, string email, string fullName, string status, IList<string> roles);
    string GenerateRefreshToken();
    string HashToken(string token);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
