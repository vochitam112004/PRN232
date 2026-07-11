using System.Security.Claims;
using Hackathon.Application.DTOs.User;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Hackathon.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepo;

    public UsersController(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    /// <summary>Lấy hồ sơ của người dùng hiện tại.</summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var user = await _userRepo.FindByIdAsync(userId);
        if (user == null) return NotFound(new { error = "Không tìm thấy người dùng." });

        var roles = await _userRepo.GetRolesAsync(user);

        return Ok(MapToProfile(user, roles));
    }

    /// <summary>Cập nhật hồ sơ của người dùng hiện tại (FullName, AvatarUrl).</summary>
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var user = await _userRepo.FindByIdAsync(userId);
        if (user == null) return NotFound(new { error = "Không tìm thấy người dùng." });

        user.FullName = request.FullName.Trim();
        user.AvatarUrl = request.AvatarUrl?.Trim();
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userRepo.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        var roles = await _userRepo.GetRolesAsync(user);
        return Ok(MapToProfile(user, roles));
    }

    /// <summary>
    /// Lấy danh sách người dùng (Chỉ dành cho Ban Tổ Chức).
    /// Hỗ trợ lọc theo status và role.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = Roles.Organizer)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status = null,
        [FromQuery] string? role = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        // Parse status
        UserStatus? parsedStatus = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            var cleanStatus = status.Replace("_", "").ToLower();
            parsedStatus = cleanStatus switch
            {
                "pendingapproval" => UserStatus.PendingApproval,
                "approved" => UserStatus.Approved,
                "rejected" => UserStatus.Rejected,
                "suspended" => UserStatus.Suspended,
                _ => null
            };
        }

        var (items, totalCount) = await _userRepo.GetAllPagedAsync(parsedStatus, role, page, pageSize);

        var userList = new List<UserListResponse>();
        foreach (var user in items)
        {
            var roles = await _userRepo.GetRolesAsync(user);
            userList.Add(new UserListResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                AvatarUrl = user.AvatarUrl,
                Status = user.Status.ToString(),
                Role = roles.FirstOrDefault() ?? string.Empty,
                CreatedAt = user.CreatedAt
            });
        }

        return Ok(new UserListPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Items = userList
        });
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private Guid GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    private static UserProfileResponse MapToProfile(
        Domain.Entities.ApplicationUser user,
        IList<string> roles)
    {
        return new UserProfileResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            AvatarUrl = user.AvatarUrl,
            Status = user.Status.ToString(),
            Role = roles.FirstOrDefault() ?? string.Empty,
            CreatedAt = user.CreatedAt,
            StudentProfile = user.StudentProfile == null ? null : new StudentProfileDto
            {
                StudentCode = user.StudentProfile.StudentCode,
                IsFptStudent = user.StudentProfile.IsFptStudent,
                UniversityName = user.StudentProfile.UniversityName
            }
        };
    }
}
