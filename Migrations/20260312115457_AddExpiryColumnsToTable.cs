using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodHeroA.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiryColumnsToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("edc77b85-32f9-4753-9583-6e296a4bc8e9"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "BloodStorages",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                table: "BloodStorages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("edff0628-0eb7-4b72-b828-82e583cd5d11"), null, new DateTime(2026, 3, 12, 11, 54, 54, 277, DateTimeKind.Utc).AddTicks(3159), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$jnZnc4kCsiWFFuhVv/GD2e3JNse4knpx2BhkLeDKn7arpuIZxPzk2", true, false, "07043138331", null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("edff0628-0eb7-4b72-b828-82e583cd5d11"));

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "BloodStorages");

            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "BloodStorages");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("edc77b85-32f9-4753-9583-6e296a4bc8e9"), null, new DateTime(2026, 3, 10, 12, 13, 44, 359, DateTimeKind.Utc).AddTicks(7534), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$Xb53aj5z.U8mCcdXbOdKjexbVBk0Ts8n4.EYvjwVpLWyauuaLKjSK", true, false, "07043138331", null, 1 });
        }
    }
}
