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

    // Auth-related tables
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<AccountApproval> AccountApprovals => Set<AccountApproval>();

    // Phase 2 Entities
    public DbSet<CriteriaTemplate> CriteriaTemplates => Set<CriteriaTemplate>();
    public DbSet<CriteriaTemplateItem> CriteriaTemplateItems => Set<CriteriaTemplateItem>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventCriteria> EventCriteria => Set<EventCriteria>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Round> Rounds => Set<Round>();
    public DbSet<RoundPromotionRule> RoundPromotionRules => Set<RoundPromotionRule>();

    // Phase 3 Entities
    public DbSet<CategoryMentor> CategoryMentors => Set<CategoryMentor>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Submission> Submissions => Set<Submission>();

    // Phase 4 Entities
    public DbSet<JudgeAssignment> JudgeAssignments => Set<JudgeAssignment>();
    public DbSet<JudgeScore> JudgeScores => Set<JudgeScore>();
    public DbSet<RoundResult> RoundResults => Set<RoundResult>();

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

        // Configure Phase 2 Entities
        ConfigureCriteriaTemplate(builder);
        ConfigureCriteriaTemplateItem(builder);
        ConfigureEvent(builder);
        ConfigureEventCriteria(builder);
        ConfigureCategory(builder);
        ConfigureRound(builder);
        ConfigureRoundPromotionRule(builder);

        // Configure Phase 3 Entities
        ConfigureCategoryMentor(builder);
        ConfigureTeam(builder);
        ConfigureTeamMember(builder);
        ConfigureSubmission(builder);

        // Configure Phase 4 Entities
        ConfigureJudgeAssignment(builder);
        ConfigureJudgeScore(builder);
        ConfigureRoundResult(builder);

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
                    v => ToSnakeCase(v),
                    v => FromSnakeCase<UserStatus>(v))
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
                    v => ToSnakeCase(v),
                    v => FromSnakeCase<UserStatus>(v));
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

    private static string ToSnakeCase<T>(T val) where T : struct, Enum
    {
        var str = val.ToString();
        return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }

    private static T FromSnakeCase<T>(string value) where T : struct, Enum
    {
        var pascal = value.Replace("_", "");
        return Enum.Parse<T>(pascal, true);
    }

    private static void ConfigureCriteriaTemplate(ModelBuilder b)
    {
        b.Entity<CriteriaTemplate>(e =>
        {
            e.ToTable("CriteriaTemplates");
            e.HasKey(t => t.Id);
            e.Property(t => t.Name).HasMaxLength(255).IsRequired();
            e.Property(t => t.Description).HasColumnType("nvarchar(max)");
            e.Property(t => t.IsDefault).HasDefaultValue(false);
            e.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(t => t.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTemplates)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCriteriaTemplateItem(ModelBuilder b)
    {
        b.Entity<CriteriaTemplateItem>(e =>
        {
            e.ToTable("CriteriaTemplateItems");
            e.HasKey(i => i.Id);
            e.Property(i => i.Name).HasMaxLength(255).IsRequired();
            e.Property(i => i.Description).HasColumnType("nvarchar(max)");
            e.Property(i => i.MaxScore).HasPrecision(5, 2).IsRequired();
            e.Property(i => i.Weight).HasPrecision(5, 4).IsRequired();
            e.Property(i => i.DisplayOrder).HasDefaultValue(0);
            e.Property(i => i.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(i => i.Template)
                .WithMany(t => t.Items)
                .HasForeignKey(i => i.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureEvent(ModelBuilder b)
    {
        b.Entity<Event>(e =>
        {
            e.ToTable("Events");
            e.HasKey(ev => ev.Id);
            e.Property(ev => ev.Title).HasMaxLength(255).IsRequired();
            e.Property(ev => ev.Description).HasColumnType("nvarchar(max)");
            e.Property(ev => ev.BannerUrl).HasColumnType("nvarchar(max)");
            e.Property(ev => ev.Status)
                .HasConversion(
                    v => ToSnakeCase(v),
                    v => FromSnakeCase<EventStatus>(v))
                .HasDefaultValue(EventStatus.Draft);
            e.Property(ev => ev.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(ev => ev.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(ev => ev.Creator)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(ev => ev.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(ev => ev.CriteriaTemplate)
                .WithMany()
                .HasForeignKey(ev => ev.CriteriaTemplateId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

    private static void ConfigureEventCriteria(ModelBuilder b)
    {
        b.Entity<EventCriteria>(e =>
        {
            e.ToTable("EventCriteria");
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).HasMaxLength(255).IsRequired();
            e.Property(c => c.Description).HasColumnType("nvarchar(max)");
            e.Property(c => c.MaxScore).HasPrecision(5, 2).IsRequired();
            e.Property(c => c.Weight).HasPrecision(5, 4).IsRequired();
            e.Property(c => c.DisplayOrder).HasDefaultValue(0);
            e.Property(c => c.IsActive).HasDefaultValue(true);
            e.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(c => c.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(c => c.Event)
                .WithMany(ev => ev.Criteria)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.SourceItem)
                .WithMany()
                .HasForeignKey(c => c.SourceItemId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

    private static void ConfigureCategory(ModelBuilder b)
    {
        b.Entity<Category>(e =>
        {
            e.ToTable("Categories");
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).HasMaxLength(255).IsRequired();
            e.Property(c => c.Description).HasColumnType("nvarchar(max)");
            e.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(c => c.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(c => c.Event)
                .WithMany(ev => ev.Categories)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureRound(ModelBuilder b)
    {
        b.Entity<Round>(e =>
        {
            e.ToTable("Rounds");
            e.HasKey(r => r.Id);
            e.Property(r => r.Name).HasMaxLength(255).IsRequired();
            e.Property(r => r.Description).HasColumnType("nvarchar(max)");
            e.Property(r => r.RoundOrder).IsRequired();
            e.Property(r => r.Status)
                .HasConversion(
                    v => ToSnakeCase(v),
                    v => FromSnakeCase<RoundStatus>(v))
                .HasDefaultValue(RoundStatus.Upcoming);
            e.Property(r => r.SubmissionDeadline).IsRequired();
            e.Property(r => r.IsCalibrationRound).HasDefaultValue(false);
            e.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(r => r.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(r => r.Event)
                .WithMany(ev => ev.Rounds)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureRoundPromotionRule(ModelBuilder b)
    {
        b.Entity<RoundPromotionRule>(e =>
        {
            e.ToTable("RoundPromotionRules");
            e.HasKey(p => p.Id);
            e.Property(p => p.RuleType)
                .HasConversion(
                    v => ToSnakeCase(v),
                    v => FromSnakeCase<PromotionRuleType>(v));
            e.Property(p => p.ScoreThreshold).HasPrecision(6, 2);
            e.Property(p => p.PerCategory).HasDefaultValue(true);
            e.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(p => p.Round)
                .WithMany(r => r.PromotionRules)
                .HasForeignKey(p => p.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureCategoryMentor(ModelBuilder b)
    {
        b.Entity<CategoryMentor>(e =>
        {
            e.ToTable("CategoryMentors");
            e.HasKey(cm => new { cm.CategoryId, cm.MentorId });
            e.Property(cm => cm.AssignedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(cm => cm.Category)
                .WithMany(c => c.CategoryMentors)
                .HasForeignKey(cm => cm.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(cm => cm.Mentor)
                .WithMany(u => u.CategoryMentorships)
                .HasForeignKey(cm => cm.MentorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureTeam(ModelBuilder b)
    {
        b.Entity<Team>(e =>
        {
            e.ToTable("Teams");
            e.HasKey(t => t.Id);
            e.Property(t => t.Name).HasMaxLength(255).IsRequired();
            e.Property(t => t.Description).HasColumnType("nvarchar(max)");
            e.Property(t => t.InviteCode).HasMaxLength(50).IsRequired();
            e.HasIndex(t => t.InviteCode).IsUnique();
            e.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(t => t.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(t => t.Category)
                .WithMany(c => c.Teams)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(t => t.Leader)
                .WithMany(u => u.LedTeams)
                .HasForeignKey(t => t.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureTeamMember(ModelBuilder b)
    {
        b.Entity<TeamMember>(e =>
        {
            e.ToTable("TeamMembers");
            e.HasKey(tm => new { tm.TeamId, tm.UserId });
            e.Property(tm => tm.JoinedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(tm => tm.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(tm => tm.User)
                .WithMany(u => u.TeamMemberships)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureSubmission(ModelBuilder b)
    {
        b.Entity<Submission>(e =>
        {
            e.ToTable("Submissions");
            e.HasKey(s => s.Id);
            e.Property(s => s.RepoUrl).HasMaxLength(1000).IsRequired();
            e.Property(s => s.DemoUrl).HasMaxLength(1000);
            e.Property(s => s.VideoUrl).HasMaxLength(1000);
            e.Property(s => s.Description).HasColumnType("nvarchar(max)");

            e.Property(s => s.RepoDescription).HasColumnType("nvarchar(max)");
            e.Property(s => s.RepoLastCommitMessage).HasColumnType("nvarchar(max)");
            e.Property(s => s.RepoPrimaryLanguage).HasMaxLength(100);

            e.Property(s => s.SubmittedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(s => s.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(s => s.Team)
                .WithMany(t => t.Submissions)
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(s => s.Round)
                .WithMany(r => r.Submissions)
                .HasForeignKey(s => s.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureJudgeAssignment(ModelBuilder b)
    {
        b.Entity<JudgeAssignment>(e =>
        {
            e.ToTable("JudgeAssignments");
            e.HasKey(ja => new { ja.RoundId, ja.JudgeId });
            e.Property(ja => ja.AssignedAt).HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(ja => ja.Round)
                .WithMany(r => r.JudgeAssignments)
                .HasForeignKey(ja => ja.RoundId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ja => ja.Judge)
                .WithMany(u => u.JudgeAssignments)
                .HasForeignKey(ja => ja.JudgeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureJudgeScore(ModelBuilder b)
    {
        b.Entity<JudgeScore>(e =>
        {
            e.ToTable("JudgeScores");
            e.HasKey(s => s.Id);
            e.Property(s => s.Score).HasPrecision(5, 2).IsRequired();
            e.Property(s => s.Comment).HasColumnType("nvarchar(max)");
            e.Property(s => s.ScoredAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(s => s.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            // A judge can score a submission's criteria only once
            e.HasIndex(s => new { s.SubmissionId, s.JudgeId, s.EventCriteriaId }).IsUnique();

            e.HasOne(s => s.Submission)
                .WithMany(sub => sub.JudgeScores)
                .HasForeignKey(s => s.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(s => s.Judge)
                .WithMany(u => u.JudgeScores)
                .HasForeignKey(s => s.JudgeId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(s => s.EventCriteria)
                .WithMany(c => c.JudgeScores)
                .HasForeignKey(s => s.EventCriteriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureRoundResult(ModelBuilder b)
    {
        b.Entity<RoundResult>(e =>
        {
            e.ToTable("RoundResults");
            e.HasKey(r => r.Id);
            e.Property(r => r.TotalScore).HasPrecision(8, 2).IsRequired();
            e.Property(r => r.Rank).IsRequired();
            e.Property(r => r.IsAdvanced).HasDefaultValue(false);
            e.Property(r => r.Note).HasColumnType("nvarchar(max)");
            e.Property(r => r.CalculatedAt).HasDefaultValueSql("GETUTCDATE()");

            // A submission has exactly one result per round
            e.HasIndex(r => new { r.RoundId, r.SubmissionId }).IsUnique();

            e.HasOne(r => r.Round)
                .WithMany(rnd => rnd.RoundResults)
                .HasForeignKey(r => r.RoundId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(r => r.Submission)
                .WithMany(sub => sub.RoundResults)
                .HasForeignKey(r => r.SubmissionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
