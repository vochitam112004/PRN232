using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[Route("api/scoring")]
[ApiController]
[Authorize]
public class ScoringController : ControllerBase
{
    private readonly IScoringService _service;

    public ScoringController(IScoringService service)
    {
        _service = service;
    }

    /// <summary>Giám khảo nộp điểm cho một bài nộp theo tiêu chí sự kiện.</summary>
    [HttpPost("submissions/{submissionId:guid}")]
    [Authorize(Roles = $"{Roles.JudgeInternal},{Roles.JudgeGuest}")]
    public async Task<IActionResult> SubmitScores(Guid submissionId, [FromBody] SubmitScoreRequest request)
    {
        var judgeId = GetCurrentUserId();
        if (judgeId == Guid.Empty)
            return Unauthorized("Không tìm thấy thông tin giám khảo.");

        try
        {
            await _service.SubmitScoresAsync(judgeId, submissionId, request);
            return Ok(new { message = "Đã lưu điểm thành công." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Lấy tất cả điểm của tất cả giám khảo cho một bài nộp.</summary>
    [HttpGet("submissions/{submissionId:guid}")]
    public async Task<IActionResult> GetScoresForSubmission(Guid submissionId)
    {
        var result = await _service.GetScoresForSubmissionAsync(submissionId);
        return Ok(result);
    }

    /// <summary>Lấy điểm của giám khảo hiện tại cho một bài nộp.</summary>
    [HttpGet("submissions/{submissionId:guid}/my-scores")]
    public async Task<IActionResult> GetMyScoreForSubmission(Guid submissionId)
    {
        var judgeId = GetCurrentUserId();
        if (judgeId == Guid.Empty)
            return Unauthorized("Không tìm thấy thông tin giám khảo.");

        var result = await _service.GetScoreByJudgeAsync(submissionId, judgeId);
        return Ok(result);
    }

    private Guid GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }
}
