using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodHeroA.Migrations
{
    /// <inheritdoc />
    public partial class IsStoredToDonationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("edff0628-0eb7-4b72-b828-82e583cd5d11"));

            migrationBuilder.AddColumn<bool>(
                name: "IsStored",
                table: "Donations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("68c875e1-462e-4948-a4e5-412ce501bafa"), null, new DateTime(2026, 3, 13, 17, 54, 58, 910, DateTimeKind.Utc).AddTicks(9055), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$1DATYAvIoBFyF8P4Ox.we.5r49LHeWg0jIQ8PcufHyOKV7c.VczlC", true, false, "07043138331", null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("68c875e1-462e-4948-a4e5-412ce501bafa"));

            migrationBuilder.DropColumn(
                name: "IsStored",
                table: "Donations");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("edff0628-0eb7-4b72-b828-82e583cd5d11"), null, new DateTime(2026, 3, 12, 11, 54, 54, 277, DateTimeKind.Utc).AddTicks(3159), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$jnZnc4kCsiWFFuhVv/GD2e3JNse4knpx2BhkLeDKn7arpuIZxPzk2", true, false, "07043138331", null, 1 });
        }
    }
}
