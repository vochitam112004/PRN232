using Hackathon.Application.DTOs.CriteriaTemplate;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/criteria-templates")]
[Authorize]
public class CriteriaTemplatesController : BaseApiController
{
    private readonly ICriteriaTemplateService _templateService;

    public CriteriaTemplatesController(ICriteriaTemplateService templateService)
    {
        _templateService = templateService;
    }

    /// <summary>Lấy danh sách tất cả các mẫu tiêu chí đánh giá.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var templates = await _templateService.GetAllAsync();
        return Ok(templates);
    }

    /// <summary>Lấy chi tiết một mẫu tiêu chí đánh giá theo ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var template = await _templateService.GetByIdAsync(id);
        if (template == null)
            return NotFound(new { error = "Không tìm thấy mẫu tiêu chí." });
        return Ok(template);
    }

    /// <summary>Tạo mới một mẫu tiêu chí đánh giá (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPost]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> Create([FromBody] CreateCriteriaTemplateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return Unauthorized();

        var template = await _templateService.CreateAsync(userId, request);
        return StatusCode(201, template);
    }

    /// <summary>Cập nhật thông tin chung của một mẫu (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCriteriaTemplateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error) = await _templateService.UpdateAsync(id, request);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Cập nhật mẫu tiêu chí thành công." });
    }

    /// <summary>Cập nhật danh sách tiêu chí con của mẫu (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPut("{id:guid}/items")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> UpdateItems(Guid id, [FromBody] UpdateCriteriaTemplateItemsRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error) = await _templateService.UpdateItemsAsync(id, request);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Cập nhật danh sách tiêu chí mẫu thành công." });
    }

    /// <summary>Thêm một tiêu chí con mới vào mẫu tiêu chí (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPost("{templateId:guid}/items")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> AddItem(Guid templateId, [FromBody] CreateCriteriaTemplateItemDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error, item) = await _templateService.AddItemAsync(templateId, request);
        if (!success)
            return BadRequest(new { error });

        return StatusCode(201, item);
    }

    /// <summary>Cập nhật thông tin của một tiêu chí con đơn lẻ trong mẫu (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpPut("{templateId:guid}/items/{itemId:guid}")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> UpdateItem(Guid templateId, Guid itemId, [FromBody] CreateCriteriaTemplateItemDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error) = await _templateService.UpdateItemAsync(templateId, itemId, request);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Cập nhật tiêu chí con thành công." });
    }

    /// <summary>Xóa một tiêu chí con đơn lẻ khỏi mẫu tiêu chí (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpDelete("{templateId:guid}/items/{itemId:guid}")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> DeleteItem(Guid templateId, Guid itemId)
    {
        var (success, error) = await _templateService.DeleteItemAsync(templateId, itemId);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Xóa tiêu chí con thành công." });
    }

    /// <summary>Xóa một mẫu tiêu chí đánh giá (Chỉ dành cho Ban Tổ Chức).</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, error) = await _templateService.DeleteAsync(id);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Xóa mẫu tiêu chí thành công." });
    }

}
