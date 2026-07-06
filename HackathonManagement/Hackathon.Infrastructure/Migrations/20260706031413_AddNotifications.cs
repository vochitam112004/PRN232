using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f5aebf3-e06c-4ae8-82ed-e2aabe5d4ede", "AQAAAAIAAYagAAAAEH+ajXvaoGSzVQO+lTnTUZhgwp0o+mcyU8ccJfYcssjiU/X3Ot9CYSBagQPwpnOGOQ==", "cf57a76d-28e4-4364-a93d-32ba79cd993c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5778ba83-8122-440d-bdf5-9e1cfb26d4c8", "AQAAAAIAAYagAAAAEMsuiLkNcGeacqJCYdY/zD9z01K4EONTBnn9vOb3jxczJWy5QIUPIdBqBNNxJr5jmA==", "c5e1100c-758f-45fc-abf2-8d1523e80675" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8cae5526-afe0-4168-b176-d1daf6d60e08", "AQAAAAIAAYagAAAAEFoJrUADRh8MkzzXR+8Y6g3EkMlkP0OWmu6hjoW1A1bq1DYM4gzBWB8YXpt66pOb/g==", "1e7e51f3-f978-4392-9f00-833a2079f41d" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22cb76ee-869d-4e07-8541-346873346ba2", "AQAAAAIAAYagAAAAEI9CM4xYAb+9Uw5CHVZVUyuE59AIECIY/DoQSJNnVueqpU1eiporH/WzrKoFvw5EbQ==", "b58c08e9-933b-44a6-99c8-278ea20f244c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dc7239e1-60c2-4fc0-b827-b900b50dd805", "AQAAAAIAAYagAAAAEE6t2+2LjfOCtgJDN+1PMawN8xVZN1O4pepjnCqZbe394TxgK3dX0JijAIVLAfmO5w==", "95036333-9fa1-475c-9274-8dc45c52f833" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9419124a-a7b4-4df0-902b-80d5ea836b11", "AQAAAAIAAYagAAAAEEQVojFUWYIh/iEbbPqJNAJT66klN9Zl4Ku8NCXiUrJ7Z/zbzigYETqP7G3KTrhPRg==", "06cd9691-bb0d-4da7-870b-cd8dcb73c9eb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

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
        }
    }
}
