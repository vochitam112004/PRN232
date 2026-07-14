using Hackathon.Application.DTOs.Auth;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize(Roles = $"{Roles.Organizer}")]
public class AccountApprovalController : BaseApiController
{
    private readonly IAccountApprovalService _approvalService;

    public AccountApprovalController(IAccountApprovalService approvalService)
    {
        _approvalService = approvalService;
    }

    /// <summary>Get all accounts pending organizer approval.</summary>
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingUsers()
    {
        var users = await _approvalService.GetPendingUsersAsync();
        return Ok(users);
    }

    /// <summary>Approve or reject a pending account.</summary>
    [HttpPost("review")]
    public async Task<IActionResult> ReviewAccount([FromBody] ApprovalActionRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewerId = GetCurrentUserId();
        if (reviewerId == Guid.Empty)
            return Unauthorized();

        var (success, error) = await _approvalService.ReviewAccountAsync(reviewerId, request);
        if (!success)
            return BadRequest(new { error });

        return Ok(new
        {
            message = request.Approve
                ? "Tài khoản đã được phê duyệt thành công."
                : "Tài khoản đã bị từ chối phê duyệt."
        });
    }

    /// <summary>Create a temporary guest judge account (organizer-only).</summary>
    [HttpPost("guest-judge")]
    public async Task<IActionResult> CreateGuestJudge([FromBody] CreateGuestJudgeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error) = await _approvalService.CreateGuestJudgeAsync(request);
        if (!success)
            return BadRequest(new { error });

        return StatusCode(201, new { message = $"Đã tạo tài khoản giám khảo khách mời cho {request.Email}." });
    }

    /// <summary>Suspend a user account and revoke all active sessions.</summary>
    [HttpPost("{userId:guid}/suspend")]
    public async Task<IActionResult> SuspendUser(Guid userId, [FromBody] SuspendRequest request)
    {
        var reviewerId = GetCurrentUserId();
        if (reviewerId == Guid.Empty)
            return Unauthorized();

        var (success, error) = await _approvalService.SuspendUserAsync(userId, reviewerId, request.Reason);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Đã khóa tài khoản người dùng thành công." });
    }

}

public record SuspendRequest([System.ComponentModel.DataAnnotations.Required] string Reason);
