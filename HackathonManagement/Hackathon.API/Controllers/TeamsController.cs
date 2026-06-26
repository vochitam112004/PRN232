using System;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Team;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/teams")]
[Authorize]
public class TeamsController : BaseApiController
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    /// <summary>Thành lập đội thi mới (Chỉ áp dụng cho Thí sinh).</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeamRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return Unauthorized();

        try
        {
            var team = await _teamService.CreateTeamAsync(userId, request);
            return StatusCode(201, team);
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

    /// <summary>Tham gia đội thi qua mã mời.</summary>
    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody] JoinTeamRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return Unauthorized();

        try
        {
            var team = await _teamService.JoinTeamAsync(userId, request);
            return Ok(team);
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

    /// <summary>Rời đội thi hiện tại.</summary>
    [HttpPost("{id:guid}/leave")]
    public async Task<IActionResult> Leave(Guid id)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return Unauthorized();

        try
        {
            var success = await _teamService.LeaveTeamAsync(userId, id);
            if (!success)
                return BadRequest(new { error = "Bạn không ở trong đội thi này." });

            return Ok(new { message = "Đã rời hoặc giải tán đội thi thành công." });
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

    /// <summary>Lấy thông tin chi tiết một đội thi.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null)
            return NotFound(new { error = "Không tìm thấy đội thi." });

        return Ok(team);
    }

    /// <summary>Lấy đội thi của sinh viên hiện tại trong sự kiện cụ thể.</summary>
    [HttpGet("student/event/{eventId:guid}")]
    public async Task<IActionResult> GetStudentTeam(Guid eventId)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return Unauthorized();

        var team = await _teamService.GetStudentTeamAsync(userId, eventId);
        if (team == null)
            return NotFound(new { error = "Bạn chưa tham gia đội thi nào trong sự kiện này." });

        return Ok(team);
    }

    /// <summary>Lấy danh sách các đội thi thuộc một hạng mục.</summary>
    [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId)
    {
        var teams = await _teamService.GetTeamsByCategoryAsync(categoryId);
        return Ok(teams);
    }

    /// <summary>Phân công Mentor vào hạng mục (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPost("categories/{categoryId:guid}/mentors")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> AssignMentor(Guid categoryId, [FromBody] AssignMentorRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error) = await _teamService.AssignMentorToCategoryAsync(categoryId, request.MentorId);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Phân công Mentor thành công." });
    }

    /// <summary>Gỡ phân công Mentor khỏi hạng mục (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpDelete("categories/{categoryId:guid}/mentors/{mentorId:guid}")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> RemoveMentor(Guid categoryId, Guid mentorId)
    {
        var (success, error) = await _teamService.RemoveMentorFromCategoryAsync(categoryId, mentorId);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Gỡ Mentor thành công." });
    }

    /// <summary>Lấy danh sách Mentor của một hạng mục.</summary>
    [HttpGet("categories/{categoryId:guid}/mentors")]
    public async Task<IActionResult> GetCategoryMentors(Guid categoryId)
    {
        var mentors = await _teamService.GetCategoryMentorsAsync(categoryId);
        return Ok(mentors);
    }
}
