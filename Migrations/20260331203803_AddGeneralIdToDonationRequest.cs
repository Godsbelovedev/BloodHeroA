using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodHeroA.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralIdToDonationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GeneralId",
                table: "DonationRequests",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dd38778b-eab3-4107-82f3-81e2c9d0f4d9"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 20, 38, 1, 101, DateTimeKind.Utc).AddTicks(5509));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralId",
                table: "DonationRequests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dd38778b-eab3-4107-82f3-81e2c9d0f4d9"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 20, 56, 20, 616, DateTimeKind.Utc).AddTicks(1346));
        }
    }
}
