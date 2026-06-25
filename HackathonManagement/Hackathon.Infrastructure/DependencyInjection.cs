using Hackathon.Application.Interfaces;
using Hackathon.Application.Services;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Hackathon.Infrastructure.Repositories;
using Hackathon.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hackathon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Database ────────────────────────────────────────────────────────────
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ── ASP.NET Core Identity ───────────────────────────────────────────────
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit           = true;
            options.Password.RequireLowercase       = true;
            options.Password.RequireUppercase       = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength         = 6;
            options.User.RequireUniqueEmail         = true;
            options.SignIn.RequireConfirmedEmail    = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddErrorDescriber<VietnameseIdentityErrorDescriber>()
        .AddDefaultTokenProviders();

        // ── JWT Authentication ──────────────────────────────────────────────────
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = jwtSettings["Issuer"],
                ValidAudience            = jwtSettings["Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(key),
                ClockSkew                = TimeSpan.Zero
            };
        });

        // ── Repositories (Infrastructure) ───────────────────────────────────────
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();
        services.AddScoped<IAccountApprovalRepository, AccountApprovalRepository>();
        services.AddScoped<ICriteriaTemplateRepository, CriteriaTemplateRepository>();
        services.AddScoped<IEventRepository, EventRepository>();

        // ── Infrastructure-specific services ────────────────────────────────────
        // TokenService stays here because JWT generation is a framework concern
        services.AddScoped<ITokenService, TokenService>();

        // ── Application Services ────────────────────────────────────────────────
        // Business logic lives in Application layer
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountApprovalService, AccountApprovalService>();
        services.AddScoped<ICriteriaTemplateService, CriteriaTemplateService>();
        services.AddScoped<IEventService, EventService>();

        return services;
    }
}
