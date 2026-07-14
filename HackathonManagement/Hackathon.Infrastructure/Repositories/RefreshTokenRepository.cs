using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _db;

    public RefreshTokenRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(RefreshToken token)
        => await _db.RefreshTokens.AddAsync(token);

    public Task<RefreshToken?> FindByHashAsync(string tokenHash)
        => _db.RefreshTokens.FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

    public Task RevokeAsync(RefreshToken token)
    {
        token.RevokedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    public async Task RevokeAllForUserAsync(Guid userId)
    {
        var tokens = await _db.RefreshTokens
            .Where(r => r.UserId == userId && r.RevokedAt == null)
            .ToListAsync();

        foreach (var t in tokens)
            t.RevokedAt = DateTime.UtcNow;
    }

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
