using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface INotificationService
{
    // Gửi 1 thông báo cho TẤT CẢ người tham gia sự kiện
    Task NotifyEventParticipantsAsync(Guid eventId, string title, string? body);

    // Người dùng xem thông báo của mình
    Task<IEnumerable<Notification>> GetMyNotificationsAsync(Guid userId);

    // Đánh dấu đã đọc
    Task MarkAsReadAsync(Guid notificationId, Guid userId);
}