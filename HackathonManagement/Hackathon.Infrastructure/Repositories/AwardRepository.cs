using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;   // đổi namespace nếu ApplicationDbContext ở chỗ khác
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class AwardRepository : IAwardRepository
{
    private readonly ApplicationDbContext _db;
    public AwardRepository(ApplicationDbContext db) => _db = db;

    public async Task<Award?> GetByIdAsync(Guid id)
        => await _db.Awards
            .Include(a => a.Recipients)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<Award>> GetByEventAsync(Guid eventId)
        => await _db.Awards
            .Include(a => a.Recipients)
            .Where(a => a.EventId == eventId)
            .ToListAsync();

    public async Task AddAsync(Award award) => await _db.Awards.AddAsync(award);

    public async Task AddRecipientAsync(AwardRecipient recipient)
        => await _db.AwardRecipients.AddAsync(recipient);

    public void Update(Award award) => _db.Awards.Update(award);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}