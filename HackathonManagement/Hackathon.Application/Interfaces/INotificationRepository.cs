using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface INotificationRepository
{
    // Lấy tất cả UserId của thành viên (kể cả leader) trong 1 sự kiện — không trùng
    Task<List<Guid>> GetParticipantUserIdsByEventAsync(Guid eventId);

    Task AddRangeAsync(IEnumerable<Notification> notifications);
    Task<IEnumerable<Notification>> GetByUserAsync(Guid userId);
    Task<Notification?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}