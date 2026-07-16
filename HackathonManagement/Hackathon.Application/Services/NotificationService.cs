using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;

    public NotificationService(INotificationRepository repo)
    {
        _repo = repo;
    }

    public async Task NotifyEventParticipantsAsync(Guid eventId, string title, string? body)
    {
        var userIds = await _repo.GetParticipantUserIdsByEventAsync(eventId);
        if (userIds.Count == 0) return;   // không ai để gửi -> thôi

        var notifications = userIds.Select(uid => new Notification
        {
            UserId = uid,
            EventId = eventId,
            Title = title,
            Body = body,
            IsRead = false
            // CreatedAt: dùng default SQL GETUTCDATE()
        });

        await _repo.AddRangeAsync(notifications);
        await _repo.SaveChangesAsync();
    }

    public async Task NotifyUsersAsync(IEnumerable<Guid> userIds, string title, string? body, Guid? eventId = null)
    {
        var distinctUserIds = userIds.Distinct().ToList();
        if (distinctUserIds.Count == 0) return;

        var notifications = distinctUserIds.Select(uid => new Notification
        {
            UserId = uid,
            EventId = eventId,
            Title = title,
            Body = body,
            IsRead = false
        });

        await _repo.AddRangeAsync(notifications);
        await _repo.SaveChangesAsync();
    }

    public async Task<IEnumerable<Notification>> GetMyNotificationsAsync(Guid userId)
        => await _repo.GetByUserAsync(userId);

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        var noti = await _repo.GetByIdAsync(notificationId)
            ?? throw new KeyNotFoundException("Không tìm thấy thông báo.");

        // Chỉ chủ sở hữu mới được đánh dấu đọc thông báo của mình
        if (noti.UserId != userId)
            throw new UnauthorizedAccessException("Không có quyền với thông báo này.");

        noti.IsRead = true;
        await _repo.SaveChangesAsync();
    }
}
