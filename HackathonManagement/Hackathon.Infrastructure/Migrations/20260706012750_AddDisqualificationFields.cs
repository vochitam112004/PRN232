using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDisqualificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DisqualifiedAt",
                table: "Teams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DisqualifiedBy",
                table: "Teams",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisqualifyReason",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisqualified",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisqualifiedAt",
                table: "Submissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DisqualifiedBy",
                table: "Submissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisqualifyReason",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisqualified",
                table: "Submissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisqualifiedAt",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DisqualifiedBy",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DisqualifyReason",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "IsDisqualified",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DisqualifiedAt",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "DisqualifiedBy",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "DisqualifyReason",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "IsDisqualified",
                table: "Submissions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f722d0a6-339e-4250-aedb-551492d38b90", "AQAAAAIAAYagAAAAED/xm+OT0qROR2K2XqKurzv5exNQNz87WRG9uvccVsTdhGGzyYuMBYz5iP0fo0sxuQ==", "288bd5a1-1f23-4904-b964-106c5b4675d0" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b16771a-67ea-433d-9402-9ef0064c11c9", "AQAAAAIAAYagAAAAECsOuvjkq3vFYuloDQCeisdQcQhc4XUBR6fOQivEpdjFkRQQv8IOY315baxbFUKStw==", "67410e2a-919d-4311-8012-38bf99cea3a1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f3933bd6-fb48-43ff-965a-185dc01a0541", "AQAAAAIAAYagAAAAEGLuwxdUshj86zZYiuVU1T1FIIXoHzMnyYLWY5YA8LgSBSCwhaZkNyJAuYcNKYoLYg==", "b07637cc-153d-4443-97f4-2020496e563a" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a570a71-8201-4eed-be42-2d19964f2f07", "AQAAAAIAAYagAAAAENgLIbIeKxAF1swXz9U1Wf4OoPtF+P3iIukfA3y0CZYxLCvEdUPHlscquoztMKiuzA==", "7af40786-5efc-4a17-9fc3-be09e50a9c59" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7646efb0-f4b7-427d-9229-82c2ffcc87b3", "AQAAAAIAAYagAAAAEJTiyWWKw5Xy0O9e/F5RTs3bOSKq78tvvgpoiTtRFrUM6TC8XisGcpiMwMw1zhCXKg==", "799103fd-1537-4eb2-a8dc-e1f71c39f3da" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa198bb7-f163-41e8-ad3f-378c160ab125", "AQAAAAIAAYagAAAAEOGPOcDTxHMuEA8a4khgSbm5U14EKBu3jSYIdw39GOkx9gp8LIRmHFi6ZJRkZQSbYw==", "ee47dfd4-5b55-4199-8b98-9a4266e892e4" });
        }
    }
}
