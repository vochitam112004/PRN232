using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize(Roles = Roles.Organizer)]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLoggerRepository _repo;

    public AuditLogsController(IAuditLoggerRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Lấy danh sách nhật ký kiểm tra (Chỉ dành cho Ban Tổ Chức).
    /// Hỗ trợ lọc theo hành động, loại đối tượng, người thực hiện và khoảng thời gian.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetLogs(
        [FromQuery] string? action = null,
        [FromQuery] string? targetType = null,
        [FromQuery] Guid? performedBy = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var (items, totalCount) = await _repo.GetByFilterAsync(
            action, targetType, performedBy, from, to, page, pageSize);

        var logs = items.Select(l => new
        {
            l.Id,
            Action = l.Action.ToString(),
            PerformedBy = l.PerformedBy,
            PerformedByName = l.PerformedByUser?.FullName ?? "Unknown",
            l.TargetType,
            l.TargetId,
            l.Payload,
            l.Reason,
            l.IpAddress,
            l.CreatedAt
        });

        return Ok(new
        {
            page,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            items = logs
        });
    }
}
