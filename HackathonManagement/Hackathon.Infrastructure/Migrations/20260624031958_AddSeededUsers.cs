using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeededUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, null, "34ccdbd9-f520-4566-a072-2ec4b337bacf", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "organizer@hackathon.com", true, "Organizer User", false, null, "ORGANIZER@HACKATHON.COM", "ORGANIZER@HACKATHON.COM", "AQAAAAIAAYagAAAAEGpp1ryaxZmDbtpXR8Yh4ZbKetd1glL7Sgs9JkzkqIQpoYit70ApQYlOBIuvGiCZcw==", null, false, "e65f6a44-6a3d-4c57-8a1d-abd6f26fdcbf", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "organizer@hackathon.com" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, null, "6e435d84-803a-4a68-8750-72cc326860ad", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.internal@hackathon.com", true, "Internal Judge", false, null, "JUDGE.INTERNAL@HACKATHON.COM", "JUDGE.INTERNAL@HACKATHON.COM", "AQAAAAIAAYagAAAAEOYbZBd/tkr6WaNfVZVkZcSGvz5ybQQxD3dula/oxmf8EXVKV9BXWdsYgHjFNSyhIg==", null, false, "5a4198d9-dd0d-4cf1-99fb-8764698fb7a7", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.internal@hackathon.com" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, null, "4894b32b-7e38-4ef8-a2a9-5dc6616cb3bb", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.guest@hackathon.com", true, "Guest Judge", false, null, "JUDGE.GUEST@HACKATHON.COM", "JUDGE.GUEST@HACKATHON.COM", "AQAAAAIAAYagAAAAEJtGglsRDejabAeX1qTLwEMC120KBDE6cgQVMRC34x4MYpkAV7qixoDKLhgimVAvDA==", null, false, "76c3390d-2134-4148-9ed1-9e3d83262b57", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "judge.guest@hackathon.com" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, null, "b226c195-c19f-4343-80ba-599566d260f2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mentor@hackathon.com", true, "Mentor User", false, null, "MENTOR@HACKATHON.COM", "MENTOR@HACKATHON.COM", "AQAAAAIAAYagAAAAEKncZ17pIcgCESxJYfaqG2LmZU78ETqSyiEQTDoeX/IabNLWrPbC1nqMhbW2OnDEug==", null, false, "6fef761c-ab04-41f3-96b5-d438eff0765f", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mentor@hackathon.com" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 0, null, "5c7cbb7e-d8f9-4e99-b02c-763958204fbf", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.fpt@hackathon.com", true, "FPT Student", false, null, "STUDENT.FPT@HACKATHON.COM", "STUDENT.FPT@HACKATHON.COM", "AQAAAAIAAYagAAAAEJH9G8vyN6NKCajieU+mPyuG1/vYYaGdx3awe0gz5fbgoO1f9ILjD3CsmSPWzy3KMQ==", null, false, "ccbdc1ab-18ae-43a6-b3a6-60d72599e3f1", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.fpt@hackathon.com" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 0, null, "97acc337-0dfa-4053-bf52-4f7d37014739", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.external@hackathon.com", true, "External Student", false, null, "STUDENT.EXTERNAL@HACKATHON.COM", "STUDENT.EXTERNAL@HACKATHON.COM", "AQAAAAIAAYagAAAAEDZZ7fKIHY8G/5zGioXwmcFPbRgO5F6+zmDJKa07enrz7IvQHFbvvqiWjq1mRUko9Q==", null, false, "72d6c3ac-7872-4c38-a878-40b83d9f23b9", "approved", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student.external@hackathon.com" }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StudentProfiles",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555556"));

            migrationBuilder.DeleteData(
                table: "StudentProfiles",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666667"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));
        }
    }
}
