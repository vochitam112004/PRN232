using System;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Submission;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/submissions")]
[Authorize]
public class SubmissionsController : BaseApiController
{
    private readonly ISubmissionService _submissionService;

    public SubmissionsController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    /// <summary>Nộp bài hoặc cập nhật bài nộp của đội thi trong vòng thi hiện tại.</summary>
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] CreateSubmissionRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return Unauthorized();

        try
        {
            var submission = await _submissionService.SubmitProjectAsync(userId, request);
            return Ok(submission);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Lấy thông tin chi tiết một bài nộp.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var submission = await _submissionService.GetSubmissionByIdAsync(id);
        if (submission == null)
            return NotFound(new { error = "Không tìm thấy bài nộp." });

        return Ok(submission);
    }

    /// <summary>Lấy toàn bộ bài nộp của một đội thi.</summary>
    [HttpGet("team/{teamId:guid}")]
    public async Task<IActionResult> GetByTeam(Guid teamId)
    {
        var submissions = await _submissionService.GetTeamSubmissionsAsync(teamId);
        return Ok(submissions);
    }

    /// <summary>Lấy toàn bộ bài nộp của một vòng thi.</summary>
    [HttpGet("round/{roundId:guid}")]
    public async Task<IActionResult> GetByRound(Guid roundId)
    {
        var submissions = await _submissionService.GetRoundSubmissionsAsync(roundId);
        return Ok(submissions);
    }
}
