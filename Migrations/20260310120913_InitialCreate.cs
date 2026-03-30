using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodHeroA.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BloodInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    StoredUnits = table.Column<int>(type: "int", nullable: false),
                    ReleasedUnits = table.Column<int>(type: "int", nullable: false),
                    ExpiredUnits = table.Column<int>(type: "int", nullable: false),
                    RecipientOrganizationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BankingOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodInventories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashPassWord = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecipientOrganizationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DonorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DonorOrganizationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BankingOrganizationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BankingOrganizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrganizationName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalDonations = table.Column<int>(type: "int", nullable: false),
                    TotalStorage = table.Column<int>(type: "int", nullable: false),
                    TotalExpired = table.Column<int>(type: "int", nullable: false),
                    TotalRelease = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankingOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankingOrganizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DonorOrganizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrganizationName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TotalRegisteredDonors = table.Column<int>(type: "int", nullable: false),
                    TotalDonations = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonorOrganizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SenderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReceiverId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subject = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecipientOrganizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrganizationName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    TotalRecievedBlood = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipientOrganizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MiddleName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    StateOfOrigin = table.Column<int>(type: "int", nullable: false),
                    HIV = table.Column<int>(type: "int", nullable: false),
                    HepatitisB = table.Column<int>(type: "int", nullable: false),
                    Syphilis = table.Column<int>(type: "int", nullable: false),
                    Cancer = table.Column<int>(type: "int", nullable: false),
                    HeartDisease = table.Column<int>(type: "int", nullable: false),
                    Hemophilic = table.Column<int>(type: "int", nullable: false),
                    IVDrugConsumer = table.Column<int>(type: "int", nullable: false),
                    ChronicDisease = table.Column<int>(type: "int", nullable: false),
                    SevereLungsDisease = table.Column<int>(type: "int", nullable: false),
                    Tattoo = table.Column<int>(type: "int", nullable: false),
                    LastDonationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextDueDonationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalDonations = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DonorOrganizationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donors_DonorOrganizations_DonorOrganizationId",
                        column: x => x.DonorOrganizationId,
                        principalTable: "DonorOrganizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Donors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DonationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RecipientOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BankingOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BloodTypeNeeded = table.Column<int>(type: "int", nullable: false),
                    UnitsRequested = table.Column<int>(type: "int", nullable: false),
                    UnitsSupplied = table.Column<int>(type: "int", nullable: false),
                    UnitsRemained = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationRequests_BankingOrganizations_BankingOrganizationId",
                        column: x => x.BankingOrganizationId,
                        principalTable: "BankingOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonationRequests_RecipientOrganizations_RecipientOrganizatio~",
                        column: x => x.RecipientOrganizationId,
                        principalTable: "RecipientOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DonorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DonorOrganizationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BloodTestResultId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BankingOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BloodStorageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UnitsDonated = table.Column<int>(type: "int", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DonationRemark = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DonatedBloodType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsTested = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsHealthy = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donations_BankingOrganizations_BankingOrganizationId",
                        column: x => x.BankingOrganizationId,
                        principalTable: "BankingOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Donations_DonorOrganizations_DonorOrganizationId",
                        column: x => x.DonorOrganizationId,
                        principalTable: "DonorOrganizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Donations_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReleasedBloods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DonationRequestId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BloodStorageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BankingOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RecipientOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UnitsReleased = table.Column<int>(type: "int", nullable: false),
                    ReasonForRelease = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReleasedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleasedBloods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleasedBloods_BankingOrganizations_BankingOrganizationId",
                        column: x => x.BankingOrganizationId,
                        principalTable: "BankingOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleasedBloods_DonationRequests_DonationRequestId",
                        column: x => x.DonationRequestId,
                        principalTable: "DonationRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReleasedBloods_RecipientOrganizations_RecipientOrganizationId",
                        column: x => x.RecipientOrganizationId,
                        principalTable: "RecipientOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BloodStorages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    DonationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReleasedId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DonationRequestId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StorageLocation = table.Column<int>(type: "int", nullable: false),
                    BankingOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitStored = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiryProcess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsReleased = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodStorages_BankingOrganizations_BankingOrganizationId",
                        column: x => x.BankingOrganizationId,
                        principalTable: "BankingOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodStorages_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BloodTestResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    HIV = table.Column<int>(type: "int", nullable: false),
                    HepatitisB = table.Column<int>(type: "int", nullable: false),
                    Syphilis = table.Column<int>(type: "int", nullable: false),
                    Cancer = table.Column<int>(type: "int", nullable: false),
                    HeartDisease = table.Column<int>(type: "int", nullable: false),
                    Hemophilic = table.Column<int>(type: "int", nullable: false),
                    IVDrugConsumer = table.Column<int>(type: "int", nullable: false),
                    ChronicDisease = table.Column<int>(type: "int", nullable: false),
                    SevereLungsDisease = table.Column<int>(type: "int", nullable: false),
                    Tattoo = table.Column<int>(type: "int", nullable: false),
                    DonationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BankingOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsHealthy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TestRemark = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodTestResults_BankingOrganizations_BankingOrganizationId",
                        column: x => x.BankingOrganizationId,
                        principalTable: "BankingOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodTestResults_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BankingOrganizationId", "CreatedAt", "DonorId", "DonorOrganizationId", "Email", "FullName", "HashPassWord", "IsAvailable", "IsDeleted", "PhoneNumber", "RecipientOrganizationId", "Role" },
                values: new object[] { new Guid("23606aaf-c791-4b10-b47a-9461c383f65e"), null, new DateTime(2026, 3, 10, 12, 9, 11, 635, DateTimeKind.Utc).AddTicks(607), null, null, "admin@bloodhero.com", "BloodHero Admin", "$2a$11$tIW5K23TIZqvyz/7No7FLuy42.DM.K87HnYtf7mLvbIYFh6fJNXt2", true, false, "07043138331", null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_BankingOrganizations_UserId",
                table: "BankingOrganizations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodStorages_BankingOrganizationId",
                table: "BloodStorages",
                column: "BankingOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodStorages_DonationId",
                table: "BloodStorages",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodTestResults_BankingOrganizationId",
                table: "BloodTestResults",
                column: "BankingOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodTestResults_DonationId",
                table: "BloodTestResults",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_BankingOrganizationId",
                table: "DonationRequests",
                column: "BankingOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_RecipientOrganizationId",
                table: "DonationRequests",
                column: "RecipientOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_BankingOrganizationId",
                table: "Donations",
                column: "BankingOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonorId",
                table: "Donations",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonorOrganizationId",
                table: "Donations",
                column: "DonorOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorOrganizations_UserId",
                table: "DonorOrganizations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donors_DonorOrganizationId",
                table: "Donors",
                column: "DonorOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Donors_UserId",
                table: "Donors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipientOrganizations_UserId",
                table: "RecipientOrganizations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleasedBloods_BankingOrganizationId",
                table: "ReleasedBloods",
                column: "BankingOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleasedBloods_DonationRequestId",
                table: "ReleasedBloods",
                column: "DonationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleasedBloods_RecipientOrganizationId",
                table: "ReleasedBloods",
                column: "RecipientOrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodInventories");

            migrationBuilder.DropTable(
                name: "BloodStorages");

            migrationBuilder.DropTable(
                name: "BloodTestResults");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ReleasedBloods");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "DonationRequests");

            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropTable(
                name: "BankingOrganizations");

            migrationBuilder.DropTable(
                name: "RecipientOrganizations");

            migrationBuilder.DropTable(
                name: "DonorOrganizations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
