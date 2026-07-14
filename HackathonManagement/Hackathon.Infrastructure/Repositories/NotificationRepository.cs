using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _db;
    public NotificationRepository(ApplicationDbContext db) => _db = db;

    public async Task<List<Guid>> GetParticipantUserIdsByEventAsync(Guid eventId)
    {
        // Lấy các team thuộc event (qua Category.EventId)
        var teams = _db.Teams.Where(t => t.Category.EventId == eventId);

        // Thành viên
        var memberIds = teams.SelectMany(t => t.Members.Select(m => m.UserId));
        // Leader
        var leaderIds = teams.Select(t => t.LeaderId);

        // Gộp, bỏ trùng
        return await memberIds.Concat(leaderIds).Distinct().ToListAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Notification> notifications)
        => await _db.Notifications.AddRangeAsync(notifications);

    public async Task<IEnumerable<Notification>> GetByUserAsync(Guid userId)
        => await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

    public async Task<Notification?> GetByIdAsync(Guid id)
        => await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}