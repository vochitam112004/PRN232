using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IAwardRepository
{
    Task<Award?> GetByIdAsync(Guid id);
    Task<IEnumerable<Award>> GetByEventAsync(Guid eventId);   // kèm Recipients
    Task AddAsync(Award award);
    Task AddRecipientAsync(AwardRecipient recipient);
    void Update(Award award);
    Task SaveChangesAsync();
}