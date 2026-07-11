using System;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[Route("api/ranking")]
[ApiController]
[Authorize]
public class RankingController : ControllerBase
{
    private readonly IRankingService _service;

    public RankingController(IRankingService service)
    {
        _service = service;
    }

    /// <summary>Tính toán xếp hạng và xác định đội đi tiếp cho một vòng thi (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPost("rounds/{roundId:guid}/calculate")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> CalculateRanking(Guid roundId)
    {
        try
        {
            var result = await _service.CalculateRankingAndAdvanceAsync(roundId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Lấy kết quả xếp hạng của một vòng thi.</summary>
    [HttpGet("rounds/{roundId:guid}")]
    public async Task<IActionResult> GetRoundRanking(Guid roundId)
    {
        var result = await _service.GetRoundResultsAsync(roundId);
        return Ok(result);
    }

    /// <summary>Lấy xếp hạng toàn sự kiện (dựa trên kết quả vòng cuối cùng).</summary>
    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetEventRanking(Guid eventId)
    {
        try
        {
            var result = await _service.GetEventRankingAsync(eventId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Lấy xếp hạng của một hạng mục trong một vòng thi.</summary>
    [HttpGet("rounds/{roundId:guid}/categories/{categoryId:guid}")]
    public async Task<IActionResult> GetCategoryRanking(Guid roundId, Guid categoryId)
    {
        var result = await _service.GetCategoryRankingAsync(roundId, categoryId);
        return Ok(result);
    }

    /// <summary>Lấy tóm tắt điểm số theo tiêu chí của một vòng thi (phân bố, trung bình, độ lệch chuẩn).</summary>
    [HttpGet("rounds/{roundId:guid}/score-summary")]
    public async Task<IActionResult> GetScoreSummary(Guid roundId)
    {
        var result = await _service.GetRoundScoreSummaryAsync(roundId);
        return Ok(result);
    }
}
