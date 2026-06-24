using Hackathon.Application.DTOs.Auth;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class AccountApprovalService : IAccountApprovalService
{
    private readonly IUserRepository _userRepo;
    private readonly IStudentProfileRepository _studentProfileRepo;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly IAccountApprovalRepository _approvalRepo;

    public AccountApprovalService(
        IUserRepository userRepo,
        IStudentProfileRepository studentProfileRepo,
        IRefreshTokenRepository refreshTokenRepo,
        IAccountApprovalRepository approvalRepo)
    {
        _userRepo           = userRepo;
        _studentProfileRepo = studentProfileRepo;
        _refreshTokenRepo   = refreshTokenRepo;
        _approvalRepo       = approvalRepo;
    }

    public async Task<IEnumerable<PendingUserResponse>> GetPendingUsersAsync()
    {
        var users = await _userRepo.GetByStatusAsync(UserStatus.PendingApproval);

        var result = new List<PendingUserResponse>();
        foreach (var user in users)
        {
            var roles   = await _userRepo.GetRolesAsync(user);
            var profile = await _studentProfileRepo.FindByUserIdAsync(user.Id);

            result.Add(new PendingUserResponse
            {
                Id             = user.Id,
                FullName       = user.FullName,
                Email          = user.Email!,
                Phone          = user.PhoneNumber,
                Role           = roles.FirstOrDefault() ?? string.Empty,
                Status         = user.Status.ToString(),
                IsFptStudent   = profile?.IsFptStudent ?? true,
                StudentCode    = profile?.StudentCode ?? string.Empty,
                UniversityName = profile?.UniversityName,
                RegisteredAt   = user.CreatedAt
            });
        }
        return result;
    }

    public async Task<(bool Success, string Error)> ReviewAccountAsync(Guid reviewerId, ApprovalActionRequest request)
    {
        var user = await _userRepo.FindByIdAsync(request.UserId);
        if (user == null)
            return (false, "Không tìm thấy người dùng.");

        if (user.Status != UserStatus.PendingApproval)
            return (false, $"Tài khoản không ở trạng thái chờ duyệt (trạng thái hiện tại: {user.Status}).");

        user.Status    = request.Approve ? UserStatus.Approved : UserStatus.Rejected;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        await _approvalRepo.AddAsync(new AccountApproval
        {
            Id         = Guid.NewGuid(),
            UserId     = user.Id,
            ReviewedBy = reviewerId,
            Status     = user.Status,
            Note       = request.Note,
            ReviewedAt = DateTime.UtcNow,
            CreatedAt  = DateTime.UtcNow
        });
        await _approvalRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error)> CreateGuestJudgeAsync(CreateGuestJudgeRequest request)
    {
        var existing = await _userRepo.FindByEmailAsync(request.Email);
        if (existing != null)
            return (false, "Email đã được đăng ký trên hệ thống.");

        var user = new ApplicationUser
        {
            Id          = Guid.NewGuid(),
            FullName    = request.FullName.Trim(),
            Email       = request.Email.ToLower().Trim(),
            UserName    = request.Email.ToLower().Trim(),
            PhoneNumber = request.Phone,
            Status      = UserStatus.Approved, // pre-approved by organizer
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow
        };

        var result = await _userRepo.CreateAsync(user, request.TemporaryPassword);
        if (!result.Succeeded)
            return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

        await _userRepo.AddToRoleAsync(user, Roles.JudgeGuest);
        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error)> SuspendUserAsync(Guid userId, Guid reviewerId, string reason)
    {
        var user = await _userRepo.FindByIdAsync(userId);
        if (user == null)
            return (false, "Không tìm thấy người dùng.");

        user.Status    = UserStatus.Suspended;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        // Revoke all active sessions
        await _refreshTokenRepo.RevokeAllForUserAsync(userId);

        await _approvalRepo.AddAsync(new AccountApproval
        {
            Id         = Guid.NewGuid(),
            UserId     = userId,
            ReviewedBy = reviewerId,
            Status     = UserStatus.Suspended,
            Note       = reason,
            ReviewedAt = DateTime.UtcNow,
            CreatedAt  = DateTime.UtcNow
        });
        await _approvalRepo.SaveChangesAsync();

        return (true, string.Empty);
    }
}
