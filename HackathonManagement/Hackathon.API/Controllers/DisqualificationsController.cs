using System.Security.Claims;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/disqualifications")]
[Authorize(Roles = "organizer")]   // chỉ Ban tổ chức
public class DisqualificationsController : ControllerBase
{
    private readonly IDisqualificationService _service;
    public DisqualificationsController(IDisqualificationService service) => _service = service;

    public record DisqualifyTeamRequest(Guid TeamId, string Reason);
    public record DisqualifySubmissionRequest(Guid SubmissionId, string Reason);

    [HttpPost("team")]
    public async Task<IActionResult> DisqualifyTeam(DisqualifyTeamRequest req)
    {
        await _service.DisqualifyTeamAsync(req.TeamId, GetUserId(), req.Reason, GetIp());
        return NoContent();
    }

    [HttpPost("submission")]
    public async Task<IActionResult> DisqualifySubmission(DisqualifySubmissionRequest req)
    {
        await _service.DisqualifySubmissionAsync(req.SubmissionId, GetUserId(), req.Reason, GetIp());
        return NoContent();
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id)
            ? id
            : throw new UnauthorizedAccessException("Không xác định được người dùng.");
    }

    private string? GetIp() => HttpContext.Connection.RemoteIpAddress?.ToString();
}