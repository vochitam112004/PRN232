using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAwards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_PerformedBy",
                table: "AuditLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditLogs",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Awards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AwardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrizeValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Awards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Awards_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Awards_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AwardRecipients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwardRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AwardRecipients_Awards_AwardId",
                        column: x => x.AwardId,
                        principalTable: "Awards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AwardRecipients_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "72f9c611-dfe9-4adf-a9d9-98a01168ce34", "AQAAAAIAAYagAAAAEEAOcT0wqDgWFi4SsXjw+WpJtiVmwh3vdtVlAPh6BLLMV4h/yTb9qqWdQWjoosygqg==", "f9663bbb-e596-4d73-8a9b-29c4cfbe17c9" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5c847026-67b6-4592-af3d-507a3d843ffa", "AQAAAAIAAYagAAAAEG+Zj3yPLfVioaVfEVa9saOZ1YzR0M+yu5xUlG7j9QIeLuPdCHecLHH9V7PX9m2b+Q==", "cc48b4f3-9068-4e54-b0fa-5508877a7a19" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "04d12336-35f3-4e14-b958-572d2e8be77c", "AQAAAAIAAYagAAAAEI8r6gn7g1lZ9M+R8aWfZvN9Oj9JwJ2tf8h4WOv9VoZ7b2wum5LxuKGwn/HcHzPn2Q==", "805759c7-2d91-4459-aa43-b3c598c91fd1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1aa5150-58d4-4f39-a971-cfeb97db9e5a", "AQAAAAIAAYagAAAAECM+WqcYxrsvR55nWvLC9UdVc4AMhrhaDKKhgUPpMMr9SnjcWochL98PxmNvJm6Naw==", "4289c916-67f1-41d3-a823-24e151b4fd6f" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd1067c4-9c65-4279-b969-0df0d19c1dab", "AQAAAAIAAYagAAAAEGhJJbYqNB/JNPps8n9MALvYeKuUlhr5Rpypj91+2QqIJg3u7gK4c+3QX68KSBPLDw==", "535b84f8-9dd0-4d42-a46b-5a26e326035e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa7129ba-b93a-4f44-be7e-36d01f6c458b", "AQAAAAIAAYagAAAAEGLrlsCtzvU40kmR62Vf+eweG+uu7aoD9J3G/mJR+W0cYH/VdCLDyt1e8CSd1tx+MQ==", "ad319d69-d534-4cb0-8c9f-c675d2b9db1f" });

            migrationBuilder.CreateIndex(
                name: "IX_AwardRecipients_AwardId",
                table: "AwardRecipients",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_AwardRecipients_TeamId",
                table: "AwardRecipients",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_CategoryId",
                table: "Awards",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_EventId",
                table: "Awards",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_PerformedBy",
                table: "AuditLogs",
                column: "PerformedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_PerformedBy",
                table: "AuditLogs");

            migrationBuilder.DropTable(
                name: "AwardRecipients");

            migrationBuilder.DropTable(
                name: "Awards");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditLogs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b9103bb9-c87f-49f6-917a-112489e65478", "AQAAAAIAAYagAAAAEJ9SN1NmkiHi3JEL1ydxUl2oKc18lVlEiNWqEomGeBXfWPy5IFox8JTnFgRZu6HWEQ==", "cee6fb51-4a68-4da9-8d03-dcf30bfc2b73" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8bf7a524-73f5-4815-9861-fba8465dac22", "AQAAAAIAAYagAAAAEPEboNOuJ8OSU/OqlOw8Z+a4UjTs7uQS6VBwnDjt894TANTFXM0gPWyGs4am82Thjg==", "b989f344-d4f5-49b3-a127-289ebdfbd51e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "673a55fc-d9e5-4585-8b98-33f6abd05fdb", "AQAAAAIAAYagAAAAEIn/KCGfJy5i1olqXBT46OxG5KfXoanjdor2bm9UyRvHlaQsnANsqD9cmjK+22ubyA==", "f83cfb5e-7955-4362-88e5-fd3a798d36f8" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "83fc4604-d3a7-45c7-93cf-15a006f63fde", "AQAAAAIAAYagAAAAEBdSYi7OCO7vJG0s450zUh8/aJD7AmdmTCpFLkZiI+bxruY02jGrTDjGhHqlW+C5Yg==", "67093d14-ea90-4ba0-9ae1-ce4d8fd094c8" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "746e3dc4-9e14-4ce8-a066-0ecd9168b5b8", "AQAAAAIAAYagAAAAELmRYwoasgtPblig3MJwB/Iy6Lvof4o0/faUv0hqGAwepXAo09g69tfG9wJBKh/CXQ==", "7b33b35e-cedf-49e6-81fc-01d654176a6d" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0dfa2436-de22-4583-9853-1d44bce221dc", "AQAAAAIAAYagAAAAEOWH2TwQjCu8Zk0XEZexzAonpZS2mrF42R8ma2e7Z9rrP804waDEM8e6XGr2liFiHA==", "fa974730-f3d2-463f-86a2-c2818f12a702" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_PerformedBy",
                table: "AuditLogs",
                column: "PerformedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
