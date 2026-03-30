using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodHeroA.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSomeDonorEntitiesToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("68c875e1-462e-4948-a4e5-412ce501bafa"));

            migrationBuilder.AlterColumn<int>(
                name: "Syphilis",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SevereLungsDisease",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HepatitisB",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Hemophilic",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HeartDisease",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HIV",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ChronicDisease",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Cancer",
                table: "Donors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("dd38778b-eab3-4107-82f3-81e2c9d0f4d9"), null, new DateTime(2026, 3, 13, 20, 56, 20, 616, DateTimeKind.Utc).AddTicks(1346), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$6oa/w00m3.g4NkrAqjNH9eORe/7vKmeLs4Cpn2xvJLGE3zq4TKlmG", true, false, "07043138331", null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dd38778b-eab3-4107-82f3-81e2c9d0f4d9"));

            migrationBuilder.AlterColumn<int>(
                name: "Syphilis",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SevereLungsDisease",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HepatitisB",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Hemophilic",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HeartDisease",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HIV",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChronicDisease",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Cancer",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("68c875e1-462e-4948-a4e5-412ce501bafa"), null, new DateTime(2026, 3, 13, 17, 54, 58, 910, DateTimeKind.Utc).AddTicks(9055), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$1DATYAvIoBFyF8P4Ox.we.5r49LHeWg0jIQ8PcufHyOKV7c.VczlC", true, false, "07043138331", null, 1 });
        }
    }
}
