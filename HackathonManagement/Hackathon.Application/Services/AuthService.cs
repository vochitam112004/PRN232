using Hackathon.Application.DTOs.Auth;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Constants;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Hackathon.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly IStudentProfileRepository _studentProfileRepo;
    private readonly IAccountApprovalRepository _approvalRepo;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public AuthService(
        IUserRepository userRepo,
        IRefreshTokenRepository refreshTokenRepo,
        IStudentProfileRepository studentProfileRepo,
        IAccountApprovalRepository approvalRepo,
        ITokenService tokenService,
        IConfiguration config)
    {
        _userRepo          = userRepo;
        _refreshTokenRepo  = refreshTokenRepo;
        _studentProfileRepo = studentProfileRepo;
        _approvalRepo      = approvalRepo;
        _tokenService      = tokenService;
        _config            = config;
    }

    public async Task<(bool Success, string[] Errors)> RegisterAsync(RegisterRequest request)
    {
        if (!request.IsFptStudent && string.IsNullOrWhiteSpace(request.UniversityName))
            return (false, new[] { "Tên trường đại học là bắt buộc đối với sinh viên ngoài trường." });

        var user = new ApplicationUser
        {
            Id          = Guid.NewGuid(),
            FullName    = request.FullName.Trim(),
            Email       = request.Email.ToLower().Trim(),
            UserName    = request.Email.ToLower().Trim(),
            PhoneNumber = request.Phone,
            Status      = UserStatus.PendingApproval,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow
        };

        var result = await _userRepo.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description).ToArray());

        var role = request.IsFptStudent ? Roles.StudentFpt : Roles.StudentExternal;
        await _userRepo.AddToRoleAsync(user, role);

        await _studentProfileRepo.AddAsync(new StudentProfile
        {
            Id             = Guid.NewGuid(),
            UserId         = user.Id,
            StudentCode    = request.StudentCode.Trim(),
            IsFptStudent   = request.IsFptStudent,
            UniversityName = request.IsFptStudent ? null : request.UniversityName?.Trim(),
            CreatedAt      = DateTime.UtcNow
        });

        await _approvalRepo.AddAsync(new AccountApproval
        {
            Id        = Guid.NewGuid(),
            UserId    = user.Id,
            Status    = UserStatus.PendingApproval,
            CreatedAt = DateTime.UtcNow
        });

        await _studentProfileRepo.SaveChangesAsync();
        return (true, Array.Empty<string>());
    }

    public async Task<(bool Success, AuthResponse? Response, string Error)> LoginAsync(LoginRequest request)
    {
        var user = await _userRepo.FindByEmailAsync(request.Email);
        if (user == null || !await _userRepo.CheckPasswordAsync(user, request.Password))
            return (false, null, "Email hoặc mật khẩu không chính xác.");

        if (user.Status == UserStatus.PendingApproval)
            return (false, null, "Tài khoản của bạn đang chờ Ban tổ chức phê duyệt.");
        if (user.Status == UserStatus.Rejected)
            return (false, null, "Đăng ký tài khoản của bạn đã bị từ chối.");
        if (user.Status == UserStatus.Suspended)
            return (false, null, "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ Ban tổ chức.");

        var roles        = await _userRepo.GetRolesAsync(user);
        var accessToken  = _tokenService.GenerateAccessToken(user.Id, user.Email!, user.FullName, user.Status.ToString(), roles);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var expireDays = int.Parse(_config["JwtSettings:RefreshTokenExpiryDays"] ?? "7");
        await _refreshTokenRepo.AddAsync(new RefreshToken
        {
            Id        = Guid.NewGuid(),
            UserId    = user.Id,
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(expireDays),
            CreatedAt = DateTime.UtcNow
        });

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        return (true, BuildAuthResponse(user, roles, accessToken, refreshToken), string.Empty);
    }

    public async Task<(bool Success, AuthResponse? Response, string Error)> RefreshTokenAsync(string refreshToken)
    {
        var tokenHash = _tokenService.HashToken(refreshToken);
        var stored    = await _refreshTokenRepo.FindByHashAsync(tokenHash);

        if (stored == null || !stored.IsActive)
            return (false, null, "Refresh token không hợp lệ hoặc đã hết hạn.");

        var user = await _userRepo.FindByIdAsync(stored.UserId);
        if (user == null)
            return (false, null, "Không tìm thấy người dùng.");

        var roles = await _userRepo.GetRolesAsync(user);

        // Rotate refresh token
        await _refreshTokenRepo.RevokeAsync(stored);

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var expireDays      = int.Parse(_config["JwtSettings:RefreshTokenExpiryDays"] ?? "7");
        await _refreshTokenRepo.AddAsync(new RefreshToken
        {
            Id        = Guid.NewGuid(),
            UserId    = user.Id,
            TokenHash = _tokenService.HashToken(newRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(expireDays),
            CreatedAt = DateTime.UtcNow
        });
        await _refreshTokenRepo.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email!, user.FullName, user.Status.ToString(), roles);
        return (true, BuildAuthResponse(user, roles, accessToken, newRefreshToken), string.Empty);
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        var stored = await _refreshTokenRepo.FindByHashAsync(_tokenService.HashToken(refreshToken));
        if (stored == null || !stored.IsActive) return false;
        await _refreshTokenRepo.RevokeAsync(stored);
        await _refreshTokenRepo.SaveChangesAsync();
        return true;
    }

    private AuthResponse BuildAuthResponse(ApplicationUser user, IList<string> roles, string accessToken, string refreshToken)
    {
        var expireMinutes = int.Parse(_config["JwtSettings:AccessTokenExpiryMinutes"] ?? "60");
        return new AuthResponse
        {
            AccessToken  = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt    = DateTime.UtcNow.AddMinutes(expireMinutes),
            User = new UserInfo
            {
                Id       = user.Id,
                FullName = user.FullName,
                Email    = user.Email!,
                Role     = roles.FirstOrDefault() ?? string.Empty,
                Status   = user.Status.ToString()
            }
        };
    }
}
