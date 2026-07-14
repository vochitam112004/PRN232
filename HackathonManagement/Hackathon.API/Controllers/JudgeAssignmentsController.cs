using System;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Assuming Organizer/Admin is checked via policy, simplify for now
public class JudgeAssignmentsController : ControllerBase
{
    private readonly IJudgeAssignmentService _service;

    public JudgeAssignmentsController(IJudgeAssignmentService service)
    {
        _service = service;
    }

    [HttpPost("rounds/{roundId}/judges")]
    public async Task<IActionResult> AssignJudge(Guid roundId, [FromBody] AssignJudgeRequest request)
    {
        await _service.AssignJudgeToRoundAsync(roundId, request.JudgeId);
        return Ok(new { Message = "Phân công giám khảo thành công." });
    }

    [HttpDelete("rounds/{roundId}/judges/{judgeId}")]
    public async Task<IActionResult> RemoveJudge(Guid roundId, Guid judgeId)
    {
        await _service.RemoveJudgeFromRoundAsync(roundId, judgeId);
        return Ok(new { Message = "Đã huỷ phân công giám khảo." });
    }

    [HttpGet("rounds/{roundId}")]
    public async Task<IActionResult> GetAssignmentsByRound(Guid roundId)
    {
        var result = await _service.GetAssignmentsByRoundAsync(roundId);
        return Ok(result);
    }

    [HttpGet("judges/{judgeId}")]
    public async Task<IActionResult> GetAssignmentsByJudge(Guid judgeId)
    {
        var result = await _service.GetAssignmentsByJudgeAsync(judgeId);
        return Ok(result);
    }
}
