namespace Hackathon.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }              // người nhận (ApplicationUser)
    public Guid? EventId { get; set; }            // null = thông báo chung
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }       // để trơn, dùng default SQL (giống các entity khác)
}