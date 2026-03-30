using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodHeroA.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("23606aaf-c791-4b10-b47a-9461c383f65e"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("edc77b85-32f9-4753-9583-6e296a4bc8e9"), null, new DateTime(2026, 3, 10, 12, 13, 44, 359, DateTimeKind.Utc).AddTicks(7534), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$Xb53aj5z.U8mCcdXbOdKjexbVBk0Ts8n4.EYvjwVpLWyauuaLKjSK", true, false, "07043138331", null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("edc77b85-32f9-4753-9583-6e296a4bc8e9"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("23606aaf-c791-4b10-b47a-9461c383f65e"), null, new DateTime(2026, 3, 10, 12, 9, 11, 635, DateTimeKind.Utc).AddTicks(607), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$tIW5K23TIZqvyz/7No7FLuy42.DM.K87HnYtf7mLvbIYFh6fJNXt2", true, false, "07043138331", null, 1 });
        }
    }
}
