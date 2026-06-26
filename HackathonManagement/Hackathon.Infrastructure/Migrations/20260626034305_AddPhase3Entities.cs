using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase3Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryMentors",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryMentors", x => new { x.CategoryId, x.MentorId });
                    table.ForeignKey(
                        name: "FK_CategoryMentors_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryMentors_Users_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InviteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_Users_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DemoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepoDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepoStars = table.Column<int>(type: "int", nullable: true),
                    RepoLastCommitMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepoLastCommitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepoPrimaryLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => new { x.TeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c462778d-f8a4-4dbe-b13b-993c7492d5e0", "AQAAAAIAAYagAAAAEGj8ci2ACh74YRdyqdD2zwkT5McXdLHbGMH8bJRkbj19/euZ1jrX2TZuzFoh1tPrUQ==", "f78fb69f-b4bb-436b-aad3-3d67aeb1393c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "47abf222-a868-41c3-b459-49ae9c2bfd96", "AQAAAAIAAYagAAAAEILkinRplA69jklNZRRs9DDL9ROMCTIlicO2GPVRGFZu/ZYkGgnjJJqgaFIm3ALrgA==", "a56cab15-3e3c-46c2-a686-ecc8acaa3fb8" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f095278-4ba5-449b-9d11-66f170e46404", "AQAAAAIAAYagAAAAEFXGINFXstPJngXt+wmOLNkdNxEif7jiwBo3fIH4s0Uje+t2hOJ3YBPXD1I3u2EM0w==", "55f077c3-6416-4e3f-824b-280916648867" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "94eef476-afae-4c72-a8c9-16dbaf9b36f6", "AQAAAAIAAYagAAAAEN9+xEpimB+LDMcLqL4qJ32DGuNJw++uBeFf/+qeloAMCad2EgF5xqGH7/KZf7lSSw==", "d930f8ba-1327-4507-9745-de04a10777aa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "65055e16-83fc-442b-8691-132caa413440", "AQAAAAIAAYagAAAAEJAfG2aL4WspOYPG6xHgiyhGI4js7V9pYOzn52mRGva7B0uI/GxTIgKUO7PnNOr8Pw==", "2ff7b1d1-1fc5-4e9f-9409-5071f69ebd64" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f587f411-a1dc-48a4-a222-9e95f46bd942", "AQAAAAIAAYagAAAAEHXdhTGK2MO7bc2nY/WjupNbVzg50l4wTeSnwXZkG6L9f+8zmmQDMBjWm3S24WOAAA==", "6db9ec44-ee26-4184-8792-b1284d56c6d9" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMentors_MentorId",
                table: "CategoryMentors",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_RoundId",
                table: "Submissions",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_TeamId",
                table: "Submissions",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CategoryId",
                table: "Teams",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_InviteCode",
                table: "Teams",
                column: "InviteCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeaderId",
                table: "Teams",
                column: "LeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryMentors");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "73e3df04-8eef-498c-8408-5c8dc68c82b6", "AQAAAAIAAYagAAAAEFEtaMLbC5kKim3P7q/EO15BtrzTxLQGPfAf1mMtYNffpoZjeNMP0qlG+KvfIFzAnw==", "ddda77d6-1bfd-457b-a756-f33c9d83e70e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7dedcde1-7fe4-477d-8168-258e1aa98c6c", "AQAAAAIAAYagAAAAENitIutki8RtXmTKInbGUKKIVm8RNub4tehfDxfOj7ginEjKijb4PXiXL0UYvsJuaA==", "b7c14e72-3bf8-4101-9fc9-bfbf09637a95" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "168a30a0-82fe-46fe-8da8-af97c02aeb94", "AQAAAAIAAYagAAAAENHoqB/DVn6OlQIotvKffXvtAqBzSpupqGpswY1Kzswbd5INCHlLDPQf1CkwpI9znQ==", "e062ac56-473f-4a04-b11b-ae5d9d04d7c9" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4cb6fbf-af10-45aa-8b0b-dd97888acdd4", "AQAAAAIAAYagAAAAEAwDMOVSNGVFKkcnWMOnGhBfz/Ktq7YGWNr8vkkNScfs+shQ9DdlteJKOes/0EOr4g==", "f668afcc-07c7-48e1-9b08-ff3a92fc7019" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ed9a9f4f-c405-4679-b1d9-cf0095bb4935", "AQAAAAIAAYagAAAAEA9JCZqXbQy0MNCf8lrnNVkNrMoL9ggzVnlnr3XW4fYproj5w6oiqT/CKlHXcCRknw==", "89dd8a4b-ec96-4e6e-89ea-b6fd15889b21" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ca753474-bc61-4d5e-868c-461a40b9d4b2", "AQAAAAIAAYagAAAAEEhM525iQ7a1LffQw/WehJoHTr2+Nch4Renp9RBVZQavCawNdMy8QqTQWAMA3bOYCg==", "4f7c4e25-6705-4d91-b588-b5cceaf4f7d6" });
        }
    }
}
