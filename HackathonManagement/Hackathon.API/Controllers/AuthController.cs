using Hackathon.Application.DTOs.Auth;
using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Register as FPT student or external student. Account requires organizer approval before login.</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, errors) = await _authService.RegisterAsync(request);
        if (!success)
            return BadRequest(new { errors });

        return Ok(new
        {
            message = "Đăng ký thành công. Tài khoản của bạn đang chờ Ban tổ chức phê duyệt."
        });
    }

    /// <summary>Login with email and password. Returns JWT access token and refresh token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, response, error) = await _authService.LoginAsync(request);
        if (!success)
            return Unauthorized(new { error });

        return Ok(response);
    }

    /// <summary>Get new access token using refresh token (token rotation).</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, response, error) = await _authService.RefreshTokenAsync(request.RefreshToken);
        if (!success)
            return Unauthorized(new { error });

        return Ok(response);
    }

    /// <summary>Logout — revoke the refresh token.</summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        var revoked = await _authService.RevokeTokenAsync(request.RefreshToken);
        return Ok(new { message = revoked ? "Đăng xuất thành công." : "Token đã bị hủy hoặc không tồn tại." });
    }
}
