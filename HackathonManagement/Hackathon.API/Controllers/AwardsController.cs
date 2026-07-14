using System.Security.Claims;
using Hackathon.Application.DTOs.Award;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/awards")]
[Authorize(Roles = "organizer")]
public class AwardsController : ControllerBase
{
    private readonly IAwardService _awardService;
    public AwardsController(IAwardService awardService) => _awardService = awardService;

    // Gợi ý top theo xếp hạng
    [HttpGet("events/{eventId:guid}/suggestions")]
    public async Task<IActionResult> Suggest(Guid eventId)
        => Ok(await _awardService.SuggestAsync(eventId));

    // BTC xác nhận trao
    [HttpPost("grant")]
    public async Task<IActionResult> Grant(GrantAwardRequest request)
    {
        await _awardService.GrantAsync(request, GetUserId());
        return NoContent();
    }

    // Xem giải đã trao
    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetByEvent(Guid eventId)
        => Ok(await _awardService.GetByEventAsync(eventId));

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id)
            ? id
            : throw new UnauthorizedAccessException("Không xác định được người dùng.");
    }
}