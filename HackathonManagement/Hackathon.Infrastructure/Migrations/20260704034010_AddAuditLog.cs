using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JudgeAssignments",
                columns: table => new
                {
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JudgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JudgeAssignments", x => new { x.RoundId, x.JudgeId });
                    table.ForeignKey(
                        name: "FK_JudgeAssignments_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JudgeAssignments_Users_JudgeId",
                        column: x => x.JudgeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JudgeScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JudgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventCriteriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScoredAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JudgeScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JudgeScores_EventCriteria_EventCriteriaId",
                        column: x => x.EventCriteriaId,
                        principalTable: "EventCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JudgeScores_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JudgeScores_Users_JudgeId",
                        column: x => x.JudgeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoundResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    IsAdvanced = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundResults_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoundResults_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9977a6e-dab7-42d7-9741-b5b4360996f6", "AQAAAAIAAYagAAAAEBJgMIygVsbSxOIOOvpKZo38XuffoTQize7xXy2xSl/vesAj7/lYHi0lSeQOWzJxgw==", "2679e0c1-87fd-4968-b64e-f3313b22ef3c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da47cf7e-509f-4736-97b2-cac92303d1bc", "AQAAAAIAAYagAAAAEKxAcsM2BjD0EaZCfJMviV2F+Jj+ufNogMLG6FTUmVpqU1HJE3BvVW/VQfhagSmYtg==", "85f93fb6-c128-428f-8672-e12051fa6048" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61904178-07e7-4a38-8437-6f46193a76be", "AQAAAAIAAYagAAAAEEhql3Tb7UJzojx7wUgaogAk+fJc+Lx8TQNuw18Y/VZZPHElPKhOtHTiK3pyy4xZFw==", "d680546b-90a2-4fff-9af6-234c211da220" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c11e57b4-0b05-4011-8bc8-8c7ef97d665f", "AQAAAAIAAYagAAAAECIl6w8C0mz9tKvOTABhU29pkVcouK0VhRCitX6P4MXw8c4+EIla8U3wIpza1GrYig==", "bcfed20b-c5c4-4687-8aed-296b4d88a589" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "82af4878-7276-4311-afb5-9ad81192b0f5", "AQAAAAIAAYagAAAAEELo3AJOnn+fN3wSJqiikubP8k149F66+PWckjOe4I5cFlpRSYgw7L3kpFsEesT1JA==", "d82458bb-db23-45df-92a6-a441f32513f3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cdf95b5d-fcef-436f-ac7c-9075759c65e4", "AQAAAAIAAYagAAAAEID4QHMEYOOKC65P4pAJ8T3vwToRL5KcPOgkX+dI2AQYq2w/OnyEUXHNnyjHP2xCeQ==", "b3021180-a476-4568-993d-178f875ceea6" });

            migrationBuilder.CreateIndex(
                name: "IX_JudgeAssignments_JudgeId",
                table: "JudgeAssignments",
                column: "JudgeId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgeScores_EventCriteriaId",
                table: "JudgeScores",
                column: "EventCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgeScores_JudgeId",
                table: "JudgeScores",
                column: "JudgeId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgeScores_SubmissionId_JudgeId_EventCriteriaId",
                table: "JudgeScores",
                columns: new[] { "SubmissionId", "JudgeId", "EventCriteriaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoundResults_RoundId_SubmissionId",
                table: "RoundResults",
                columns: new[] { "RoundId", "SubmissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoundResults_SubmissionId",
                table: "RoundResults",
                column: "SubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "JudgeAssignments");

            migrationBuilder.DropTable(
                name: "JudgeScores");

            migrationBuilder.DropTable(
                name: "RoundResults");

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
        }
    }
}
