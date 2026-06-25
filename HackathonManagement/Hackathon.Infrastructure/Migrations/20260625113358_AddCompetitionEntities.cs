using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompetitionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "pending_approval"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountApprovals_Users_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountApprovals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CriteriaTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriteriaTemplates_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UniversityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsFptStudent = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CriteriaTemplateItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaTemplateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriteriaTemplateItems_CriteriaTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "CriteriaTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "draft"),
                    RegistrationStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriteriaTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_CriteriaTemplates_CriteriaTemplateId",
                        column: x => x.CriteriaTemplateId,
                        principalTable: "CriteriaTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Events_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxTeams = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SourceItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventCriteria_CriteriaTemplateItems_SourceItemId",
                        column: x => x.SourceItemId,
                        principalTable: "CriteriaTemplateItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EventCriteria_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoundOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "upcoming"),
                    SubmissionStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmissionDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JudgingStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JudgingEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCalibrationRound = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoundPromotionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RuleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopN = table.Column<int>(type: "int", nullable: true),
                    ScoreThreshold = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    PerCategory = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundPromotionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundPromotionRules_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "organizer", "ORGANIZER" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge_internal", "JUDGE_INTERNAL" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge_guest", "JUDGE_GUEST" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mentor", "MENTOR" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student_fpt", "STUDENT_FPT" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student_external", "STUDENT_EXTERNAL" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, null, "e333c594-e69b-4a52-adc9-61d7f38863bc", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "organizer@hackathon.com", true, "Ban Tổ Chức Hackathon", false, null, "ORGANIZER@HACKATHON.COM", "ORGANIZER@HACKATHON.COM", "AQAAAAIAAYagAAAAECmt7fHWHw6VYxbVNC1snifYlIfb0lUKEbc1zm7q9SyHwxxQuVehbLhri4vaBuzuUQ==", null, false, "02fca1f6-2854-41be-a697-492a35f2c600", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "organizer@hackathon.com" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, null, "b17c58cb-e0a1-42b4-aa31-eb88b8d5eeac", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.internal@hackathon.com", true, "Giám Khảo Nội Bộ", false, null, "JUDGE.INTERNAL@HACKATHON.COM", "JUDGE.INTERNAL@HACKATHON.COM", "AQAAAAIAAYagAAAAEJ7hIbn1keAY7d7zZI+MCNQF+b4cGtpWR7lUFTnpc4K07V1ssw2THc6PxShHaeyu2w==", null, false, "74e82b29-e17c-4666-95df-628506d75597", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.internal@hackathon.com" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, null, "3de58733-ed5f-4e77-a5c1-bfe35ae53ea2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.guest@hackathon.com", true, "Giám Khảo Khách Mời", false, null, "JUDGE.GUEST@HACKATHON.COM", "JUDGE.GUEST@HACKATHON.COM", "AQAAAAIAAYagAAAAEBO7OM0mLsqYjORewmmJtoGhDIOFMzR9MavDZsj1SZh1ZvTMr1+hYfmNi38S87lzSA==", null, false, "735638e5-c5ac-420d-8ac9-02d35082ef02", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.guest@hackathon.com" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, null, "2c2336aa-0813-4ee9-93f6-1e7055ddf71c", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mentor@hackathon.com", true, "Cố Vấn Chuyên Môn", false, null, "MENTOR@HACKATHON.COM", "MENTOR@HACKATHON.COM", "AQAAAAIAAYagAAAAEHh58oEMuzdmoJPlNVBn8sPeuYRXzXM2pydLENtpzLaIM0N/kc9pvN/kBPYKkkdtZg==", null, false, "498a87b4-609b-458e-a002-5d6508591cbf", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mentor@hackathon.com" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 0, null, "ff1ee6c6-0ec4-4de4-84cc-3bc580572241", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.fpt@hackathon.com", true, "Sinh Viên FPT", false, null, "STUDENT.FPT@HACKATHON.COM", "STUDENT.FPT@HACKATHON.COM", "AQAAAAIAAYagAAAAEGzsHW3aVsf0ENREl5/CvmAzlvLXMj3sYVpKsdGB4opfRn4+uZkV3wD4LVwSUNVqcA==", null, false, "3ed80d05-4d06-41fe-abe8-805d2284d8c6", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.fpt@hackathon.com" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 0, null, "a4ae44bd-c498-4162-9a9f-ac91beb3c45a", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.external@hackathon.com", true, "Sinh Viên Ngoài Trường", false, null, "STUDENT.EXTERNAL@HACKATHON.COM", "STUDENT.EXTERNAL@HACKATHON.COM", "AQAAAAIAAYagAAAAENJau20XtHlIAj0vXFqakemFJCwQcNfFf1IwUe9jMbiBBPbyR2kmXTtwIlDPPA+OYw==", null, false, "baa03523-9222-4037-bc26-714530acf75d", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.external@hackathon.com" }
                });

            migrationBuilder.InsertData(
                table: "StudentProfiles",
                columns: new[] { "Id", "CreatedAt", "IsFptStudent", "StudentCode", "UniversityName", "UserId" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555556"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "SE160001", null, new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.InsertData(
                table: "StudentProfiles",
                columns: new[] { "Id", "CreatedAt", "StudentCode", "UniversityName", "UserId" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666667"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EXT16002", "VNU University", new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new Guid("66666666-6666-6666-6666-666666666666") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountApprovals_ReviewedBy",
                table: "AccountApprovals",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AccountApprovals_UserId",
                table: "AccountApprovals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_EventId",
                table: "Categories",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaTemplateItems_TemplateId",
                table: "CriteriaTemplateItems",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaTemplates_CreatedBy",
                table: "CriteriaTemplates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EventCriteria_EventId",
                table: "EventCriteria",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventCriteria_SourceItemId",
                table: "EventCriteria",
                column: "SourceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedBy",
                table: "Events",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CriteriaTemplateId",
                table: "Events",
                column: "CriteriaTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoundPromotionRules_RoundId",
                table: "RoundPromotionRules",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_EventId",
                table: "Rounds",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_UserId",
                table: "StudentProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountApprovals");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "EventCriteria");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "RoundPromotionRules");

            migrationBuilder.DropTable(
                name: "StudentProfiles");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "CriteriaTemplateItems");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "CriteriaTemplates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
