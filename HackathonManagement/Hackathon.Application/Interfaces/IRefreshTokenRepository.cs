using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> FindByHashAsync(string tokenHash);
    Task RevokeAsync(RefreshToken token);
    Task RevokeAllForUserAsync(Guid userId);
    Task SaveChangesAsync();
}
