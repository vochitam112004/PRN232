using System;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RankingController : ControllerBase
{
    private readonly IRankingService _service;

    public RankingController(IRankingService service)
    {
        _service = service;
    }

    [HttpPost("rounds/{roundId}/calculate")]
    public async Task<IActionResult> CalculateRanking(Guid roundId)
    {
        var result = await _service.CalculateRankingAndAdvanceAsync(roundId);
        return Ok(result);
    }

    [HttpGet("rounds/{roundId}")]
    public async Task<IActionResult> GetRanking(Guid roundId)
    {
        var result = await _service.GetRoundResultsAsync(roundId);
        return Ok(result);
    }
}
