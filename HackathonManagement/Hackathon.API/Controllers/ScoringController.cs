using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Should be restricted to JudgeInternal, JudgeGuest
public class ScoringController : ControllerBase
{
    private readonly IScoringService _service;

    public ScoringController(IScoringService service)
    {
        _service = service;
    }

    [HttpPost("submissions/{submissionId}")]
    public async Task<IActionResult> SubmitScores(Guid submissionId, [FromBody] SubmitScoreRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var judgeId))
        {
            return Unauthorized("Không tìm thấy thông tin giám khảo.");
        }

        await _service.SubmitScoresAsync(judgeId, submissionId, request);
        return Ok(new { Message = "Đã lưu điểm thành công." });
    }

    [HttpGet("submissions/{submissionId}")]
    public async Task<IActionResult> GetScoresForSubmission(Guid submissionId)
    {
        var result = await _service.GetScoresForSubmissionAsync(submissionId);
        return Ok(result);
    }
    
    [HttpGet("submissions/{submissionId}/my-scores")]
    public async Task<IActionResult> GetMyScoreForSubmission(Guid submissionId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var judgeId))
        {
            return Unauthorized("Không tìm thấy thông tin giám khảo.");
        }

        var result = await _service.GetScoreByJudgeAsync(submissionId, judgeId);
        return Ok(result);
    }
}
