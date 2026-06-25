using Hackathon.Application.DTOs.Event;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/events")]
[Authorize]
public class EventsController : BaseApiController
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>Lấy danh sách tất cả các sự kiện.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    /// <summary>Lấy thông tin chi tiết một sự kiện (Kèm tiêu chí, hạng mục, vòng thi).</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ev = await _eventService.GetEventByIdAsync(id);
        if (ev == null)
            return NotFound(new { error = "Không tìm thấy sự kiện." });
        return Ok(ev);
    }

    /// <summary>Tạo mới một sự kiện (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPost]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return Unauthorized();

        try
        {
            var ev = await _eventService.CreateEventAsync(userId, request);
            return StatusCode(201, ev);
        }
        catch (System.ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Cập nhật, thêm hoặc xóa các tiêu chí của sự kiện (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPut("{id:guid}/criteria")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> UpdateCriteria(Guid id, [FromBody] UpdateEventCriteriaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error) = await _eventService.UpdateEventCriteriaAsync(id, request);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Cập nhật tiêu chí sự kiện thành công." });
    }

    /// <summary>Thêm một hạng mục thi đấu vào sự kiện (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPost("{id:guid}/categories")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> CreateCategory(Guid id, [FromBody] CreateCategoryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error, category) = await _eventService.CreateCategoryAsync(id, request);
        if (!success)
            return BadRequest(new { error });

        return StatusCode(201, category);
    }

    /// <summary>Cấu hình/Thêm mới vòng thi và quy tắc đi tiếp cho vòng thi (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPost("{id:guid}/rounds")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> ConfigureRound(Guid id, [FromBody] ConfigureRoundRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error, round, isNew) = await _eventService.ConfigureRoundAsync(id, request);
        if (!success)
            return BadRequest(new { error });

        return isNew ? StatusCode(201, round) : Ok(round);
    }

    /// <summary>Xóa một sự kiện (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, error) = await _eventService.DeleteEventAsync(id);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Xóa sự kiện thành công." });
    }

}
