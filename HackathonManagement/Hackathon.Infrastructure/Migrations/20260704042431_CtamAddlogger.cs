using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CtamAddlogger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActorUserId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "AuditLogs",
                newName: "TargetId");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "AuditLogs",
                newName: "Reason");

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
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AuditLogs",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PerformedBy",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "AuditLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_PerformedBy",
                table: "AuditLogs",
                column: "PerformedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_PerformedBy",
                table: "AuditLogs",
                column: "PerformedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_PerformedBy",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_PerformedBy",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "PerformedBy",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "AuditLogs",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "AuditLogs",
                newName: "Details");

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
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "ActorUserId",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "AuditLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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
        }
    }
}
