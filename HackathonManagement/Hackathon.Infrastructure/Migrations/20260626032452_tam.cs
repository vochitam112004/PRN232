using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e333c594-e69b-4a52-adc9-61d7f38863bc", "AQAAAAIAAYagAAAAECmt7fHWHw6VYxbVNC1snifYlIfb0lUKEbc1zm7q9SyHwxxQuVehbLhri4vaBuzuUQ==", "02fca1f6-2854-41be-a697-492a35f2c600" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b17c58cb-e0a1-42b4-aa31-eb88b8d5eeac", "AQAAAAIAAYagAAAAEJ7hIbn1keAY7d7zZI+MCNQF+b4cGtpWR7lUFTnpc4K07V1ssw2THc6PxShHaeyu2w==", "74e82b29-e17c-4666-95df-628506d75597" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3de58733-ed5f-4e77-a5c1-bfe35ae53ea2", "AQAAAAIAAYagAAAAEBO7OM0mLsqYjORewmmJtoGhDIOFMzR9MavDZsj1SZh1ZvTMr1+hYfmNi38S87lzSA==", "735638e5-c5ac-420d-8ac9-02d35082ef02" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c2336aa-0813-4ee9-93f6-1e7055ddf71c", "AQAAAAIAAYagAAAAEHh58oEMuzdmoJPlNVBn8sPeuYRXzXM2pydLENtpzLaIM0N/kc9pvN/kBPYKkkdtZg==", "498a87b4-609b-458e-a002-5d6508591cbf" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff1ee6c6-0ec4-4de4-84cc-3bc580572241", "AQAAAAIAAYagAAAAEGzsHW3aVsf0ENREl5/CvmAzlvLXMj3sYVpKsdGB4opfRn4+uZkV3wD4LVwSUNVqcA==", "3ed80d05-4d06-41fe-abe8-805d2284d8c6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4ae44bd-c498-4162-9a9f-ac91beb3c45a", "AQAAAAIAAYagAAAAENJau20XtHlIAj0vXFqakemFJCwQcNfFf1IwUe9jMbiBBPbyR2kmXTtwIlDPPA+OYw==", "baa03523-9222-4037-bc26-714530acf75d" });
        }
    }
}
