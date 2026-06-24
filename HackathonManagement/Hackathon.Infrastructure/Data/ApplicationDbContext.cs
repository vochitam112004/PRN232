using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoleConstants = Hackathon.Domain.Constants.Roles;

namespace Hackathon.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Auth-related tables only
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<AccountApproval> AccountApprovals => Set<AccountApproval>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename Identity tables to match schema
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<ApplicationRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        ConfigureUser(builder);
        ConfigureRole(builder);
        ConfigureRefreshToken(builder);
        ConfigureStudentProfile(builder);
        ConfigureAccountApproval(builder);

        SeedRoles(builder);
        SeedUsers(builder);
    }

    private static void ConfigureUser(ModelBuilder b)
    {
        b.Entity<ApplicationUser>(e =>
        {
            e.Property(u => u.FullName).HasMaxLength(255).IsRequired();
            e.Property(u => u.AvatarUrl).HasColumnType("nvarchar(max)");
            e.Property(u => u.Status)
                .HasConversion(
                    v => v.ToString().ToLower(),
                    v => Enum.Parse<UserStatus>(v, true))
                .HasDefaultValue(UserStatus.PendingApproval);
            e.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(u => u.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Navigation properties
            e.HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(u => u.StudentProfile)
                .WithOne(s => s.User)
                .HasForeignKey<StudentProfile>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(u => u.AccountApprovals)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureRole(ModelBuilder b)
    {
        b.Entity<ApplicationRole>(e =>
        {
            e.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });
    }

    private static void ConfigureRefreshToken(ModelBuilder b)
    {
        b.Entity<RefreshToken>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.TokenHash).HasMaxLength(255).IsRequired();
            e.HasIndex(r => r.TokenHash).IsUnique();
            e.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });
    }

    private static void ConfigureStudentProfile(ModelBuilder b)
    {
        b.Entity<StudentProfile>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.StudentCode).HasMaxLength(50).IsRequired();
            e.Property(s => s.UniversityName).HasMaxLength(255);
            e.Property(s => s.IsFptStudent).HasDefaultValue(true);
            e.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });
    }

    private static void ConfigureAccountApproval(ModelBuilder b)
    {
        b.Entity<AccountApproval>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Status)
                .HasConversion(
                    v => v.ToString().ToLower(),
                    v => Enum.Parse<UserStatus>(v, true));
            e.Property(a => a.Note).HasColumnType("nvarchar(max)");
            e.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Reviewer FK (separate from User FK above)
            e.HasOne(a => a.Reviewer)
                .WithMany()
                .HasForeignKey(a => a.ReviewedBy)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void SeedRoles(ModelBuilder b)
    {
        var roles = new List<ApplicationRole>
        {
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = RoleConstants.Organizer,       NormalizedName = RoleConstants.Organizer.ToUpper(),       CreatedAt = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc) },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = RoleConstants.JudgeInternal,   NormalizedName = RoleConstants.JudgeInternal.ToUpper(),   CreatedAt = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc) },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = RoleConstants.JudgeGuest,      NormalizedName = RoleConstants.JudgeGuest.ToUpper(),      CreatedAt = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc) },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = RoleConstants.Mentor,          NormalizedName = RoleConstants.Mentor.ToUpper(),          CreatedAt = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc) },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Name = RoleConstants.StudentFpt,      NormalizedName = RoleConstants.StudentFpt.ToUpper(),      CreatedAt = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc) },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Name = RoleConstants.StudentExternal, NormalizedName = RoleConstants.StudentExternal.ToUpper(), CreatedAt = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc) },
        };
        b.Entity<ApplicationRole>().HasData(roles);
    }

    private static void SeedUsers(ModelBuilder b)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        var organizerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var judgeInternalId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var judgeGuestId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var mentorId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var studentFptId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var studentExternalId = Guid.Parse("66666666-6666-6666-6666-666666666666");

        var users = new List<ApplicationUser>
        {
            new()
            {
                Id = organizerId,
                FullName = "Ban Tổ Chức Hackathon",
                Email = "organizer@hackathon.com",
                NormalizedEmail = "ORGANIZER@HACKATHON.COM",
                UserName = "organizer@hackathon.com",
                NormalizedUserName = "ORGANIZER@HACKATHON.COM",
                EmailConfirmed = true,
                Status = UserStatus.Approved,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hasher.HashPassword(null!, "Password123")
            },
            new()
            {
                Id = judgeInternalId,
                FullName = "Giám Khảo Nội Bộ",
                Email = "judge.internal@hackathon.com",
                NormalizedEmail = "JUDGE.INTERNAL@HACKATHON.COM",
                UserName = "judge.internal@hackathon.com",
                NormalizedUserName = "JUDGE.INTERNAL@HACKATHON.COM",
                EmailConfirmed = true,
                Status = UserStatus.Approved,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hasher.HashPassword(null!, "Password123")
            },
            new()
            {
                Id = judgeGuestId,
                FullName = "Giám Khảo Khách Mời",
                Email = "judge.guest@hackathon.com",
                NormalizedEmail = "JUDGE.GUEST@HACKATHON.COM",
                UserName = "judge.guest@hackathon.com",
                NormalizedUserName = "JUDGE.GUEST@HACKATHON.COM",
                EmailConfirmed = true,
                Status = UserStatus.Approved,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hasher.HashPassword(null!, "Password123")
            },
            new()
            {
                Id = mentorId,
                FullName = "Cố Vấn Chuyên Môn",
                Email = "mentor@hackathon.com",
                NormalizedEmail = "MENTOR@HACKATHON.COM",
                UserName = "mentor@hackathon.com",
                NormalizedUserName = "MENTOR@HACKATHON.COM",
                EmailConfirmed = true,
                Status = UserStatus.Approved,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hasher.HashPassword(null!, "Password123")
            },
            new()
            {
                Id = studentFptId,
                FullName = "Sinh Viên FPT",
                Email = "student.fpt@hackathon.com",
                NormalizedEmail = "STUDENT.FPT@HACKATHON.COM",
                UserName = "student.fpt@hackathon.com",
                NormalizedUserName = "STUDENT.FPT@HACKATHON.COM",
                EmailConfirmed = true,
                Status = UserStatus.Approved,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hasher.HashPassword(null!, "Password123")
            },
            new()
            {
                Id = studentExternalId,
                FullName = "Sinh Viên Ngoài Trường",
                Email = "student.external@hackathon.com",
                NormalizedEmail = "STUDENT.EXTERNAL@HACKATHON.COM",
                UserName = "student.external@hackathon.com",
                NormalizedUserName = "STUDENT.EXTERNAL@HACKATHON.COM",
                EmailConfirmed = true,
                Status = UserStatus.Approved,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hasher.HashPassword(null!, "Password123")
            }
        };

        b.Entity<ApplicationUser>().HasData(users);

        var userRoles = new List<IdentityUserRole<Guid>>
        {
            new() { UserId = organizerId, RoleId = Guid.Parse("00000000-0000-0000-0000-000000000001") },
            new() { UserId = judgeInternalId, RoleId = Guid.Parse("00000000-0000-0000-0000-000000000002") },
            new() { UserId = judgeGuestId, RoleId = Guid.Parse("00000000-0000-0000-0000-000000000003") },
            new() { UserId = mentorId, RoleId = Guid.Parse("00000000-0000-0000-0000-000000000004") },
            new() { UserId = studentFptId, RoleId = Guid.Parse("00000000-0000-0000-0000-000000000005") },
            new() { UserId = studentExternalId, RoleId = Guid.Parse("00000000-0000-0000-0000-000000000006") }
        };
        b.Entity<IdentityUserRole<Guid>>().HasData(userRoles);

        var studentProfiles = new List<StudentProfile>
        {
            new()
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555556"),
                UserId = studentFptId,
                StudentCode = "SE160001",
                IsFptStudent = true,
                UniversityName = null,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666667"),
                UserId = studentExternalId,
                StudentCode = "EXT16002",
                IsFptStudent = false,
                UniversityName = "VNU University",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };
        b.Entity<StudentProfile>().HasData(studentProfiles);
    }
}
