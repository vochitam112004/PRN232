using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase4ScoringRanking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[] { "775712f6-16de-4bf0-b38b-42f202a5790e", "AQAAAAIAAYagAAAAEIihSMIa8xxJa3G+zHoiq+7vJY4NZKuWor6CQ55S7kJSOOl8bCA/fWacMrffkQ29Gw==", "c1fe30b4-498f-48eb-a668-c36c25160f62" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75949e23-34a1-43a6-bd47-9a5fc6e149dc", "AQAAAAIAAYagAAAAEKp8MAov0hqeysd8bggIqbfwaRfm2qfq50KKyOlUh+YNTU0OSgBHjaH2QKbQbgc+3A==", "9b0fc3dd-7247-4fb5-83ab-403ba034b2ac" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0a5fd8f-2591-45cf-9333-385951939e28", "AQAAAAIAAYagAAAAEHelaR+N7VpT3DWAaBicexPpavAK9D01tBBl8WUfd/Bkfk+gk1U6jeyR97oN4ulF3g==", "e38f5c54-2547-4be2-8df0-08ed1f253850" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "687654fd-600a-4acd-af04-8949f81c5525", "AQAAAAIAAYagAAAAEDyy/h85TmD0pSiKMGMePv08UEXemqzUVnEzMIJ3yYy79OPv0neDUETiJoSYeWuX4g==", "92e951b7-24e7-4851-b29d-1c1e2a9c37cd" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc02f0ee-ddc1-4e28-984e-742755115fa9", "AQAAAAIAAYagAAAAEPoscfkFk/AewH//TnI/nzbc5OBFznjaqw2ymUcUc2KE3UkkVW7e7HQ/hFyojHDykw==", "102e34ad-a8c6-4ea7-8c88-2e94720d0dca" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2d6e4244-0928-41d7-a9f7-36875dfa64da", "AQAAAAIAAYagAAAAELMdyWm7W/EBCjqeGweEUeKGtLEMMcuMvgViyLWnGVjMpQQbUtsSrT4kqfTnAKM53w==", "0dbba785-ffe7-4bb5-9248-90ec3ae35134" });

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
