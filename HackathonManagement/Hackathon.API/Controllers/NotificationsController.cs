using System.Security.Claims;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]   // mọi user đăng nhập, KHÔNG giới hạn organizer
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;
    public NotificationsController(INotificationService service) => _service = service;

    // Thông báo của tôi
    [HttpGet("me")]
    public async Task<IActionResult> GetMine()
        => Ok(await _service.GetMyNotificationsAsync(GetUserId()));

    // Đánh dấu đã đọc
    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(Guid id)
    {
        await _service.MarkAsReadAsync(id, GetUserId());
        return NoContent();
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id)
            ? id
            : throw new UnauthorizedAccessException("Không xác định được người dùng.");
    }
}