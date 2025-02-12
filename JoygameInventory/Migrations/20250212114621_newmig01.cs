using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoygameInventory.Migrations
{
    /// <inheritdoc />
    public partial class newmig01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JoyStaffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Document = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoyStaffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Licence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicenceName = table.Column<string>(type: "TEXT", nullable: false),
                    LicenceActiveDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LicenceEndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    ProductBrand = table.Column<string>(type: "TEXT", nullable: false),
                    ProductModel = table.Column<string>(type: "TEXT", nullable: false),
                    ProductBarkod = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ProductAddDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Depoda"),
                    Ram = table.Column<int>(type: "INTEGER", nullable: false),
                    Storage = table.Column<string>(type: "TEXT", nullable: false),
                    Processor = table.Column<string>(type: "TEXT", nullable: false),
                    GraphicsCard = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.UniqueConstraint("AK_Products_ProductBarkod", x => x.ProductBarkod);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenceUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicenceId = table.Column<int>(type: "INTEGER", nullable: false),
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenceUser_JoyStaffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "JoyStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceUser_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAssigments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PreviusAssigmenId = table.Column<int>(type: "INTEGER", nullable: true),
                    AssignmentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAssigments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAssigments_JoyStaffs_PreviusAssigmenId",
                        column: x => x.PreviusAssigmenId,
                        principalTable: "JoyStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAssigments_JoyStaffs_UserId",
                        column: x => x.UserId,
                        principalTable: "JoyStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAssigments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaintenanceDescription = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServiceTitle = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceAdress = table.Column<string>(type: "TEXT", nullable: true),
                    ProductBarkod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintenance_Products_ProductBarkod",
                        column: x => x.ProductBarkod,
                        principalTable: "Products",
                        principalColumn: "ProductBarkod",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaintenanceDescription = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServiceTitle = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceAdress = table.Column<string>(type: "TEXT", nullable: false),
                    ProductBarkod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceHistory_Products_ProductBarkod",
                        column: x => x.ProductBarkod,
                        principalTable: "Products",
                        principalColumn: "ProductBarkod",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userTeam_JoyStaffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "JoyStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userTeam_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssigmentHistorys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InventoryAssigmentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigmentHistorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssigmentHistorys_InventoryAssigments_InventoryAssigmentId",
                        column: x => x.InventoryAssigmentId,
                        principalTable: "InventoryAssigments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssigmentHistorys_JoyStaffs_UserId",
                        column: x => x.UserId,
                        principalTable: "JoyStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssigmentHistorys_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Madbyte", null },
                    { "2", null, "Joygame", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "abd528fb-0a2f-4a21-8892-dfe182c0b03f", "eren.sezen@joygame.com", false, "Eren", "Sezen", false, null, null, null, null, null, false, "5afa5d71-9e0f-44eb-bf1f-491c88f007fd", false, "eren_sezen" },
                    { "2", 0, "dae66b93-cd1e-472a-94fd-449dc6933136", "osman.benlice@joygame.com", false, "Jane", "Doe", false, null, null, null, null, null, false, "2528af84-9c3b-4e31-88b5-9f48b3b34dc8", false, "osman_benlice" },
                    { "3", 0, "fd195a90-39a4-40ad-93fb-2711a99033cf", "onur.unlu@joygame.com", false, "Onur", "Ünlü", false, null, null, null, null, null, false, "6badeaa7-2cb7-4346-9678-e87c784c0a0c", false, "onur.unlu" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Masaüstü", "Masaüstü" },
                    { 2, "Notebook", "Notebook" },
                    { 3, "Ekipman", "Ekipman" },
                    { 4, "Donanım", "Donanım" }
                });

            migrationBuilder.InsertData(
                table: "JoyStaffs",
                columns: new[] { "Id", "Document", "Email", "Name", "PhoneNumber", "Surname" },
                values: new object[,]
                {
                    { 1, null, "eren.sezen@joygame.com", "Eren", "555-0101", "Sezen" },
                    { 2, null, "osman.benlice@joygame.com", "Osman", "555-0102", "Benlice" },
                    { 3, null, "onur.unlu@joygame.com", "Onur", "555-0103", "Ünlü" },
                    { 4, null, "ali.karatas@joygame.com", "Ali", "555-0104", "Karataş" },
                    { 5, null, "ayse.duran@joygame.com", "Ayşe", "555-0105", "Duran" },
                    { 6, null, "fatma.ozdemir@joygame.com", "Fatma", "555-0106", "Özdemir" },
                    { 7, null, "mehmet.bayar@joygame.com", "Mehmet", "555-0107", "Bayar" },
                    { 8, null, "hasan.sahin@joygame.com", "Hasan", "555-0108", "Şahin" },
                    { 9, null, "zeynep.kucuk@joygame.com", "Zeynep", "555-0109", "Küçük" },
                    { 10, null, "yusuf.bozkurt@joygame.com", "Yusuf", "555-0110", "Bozkurt" },
                    { 11, null, "cihad.yilmazer@madbytegames.com", "Cihad", "555-0110", "Yılmazer" }
                });

            migrationBuilder.InsertData(
                table: "Licence",
                columns: new[] { "Id", "LicenceActiveDate", "LicenceEndDate", "LicenceName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Creative Cloud" },
                    { 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Photoshop" },
                    { 3, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Substance" },
                    { 4, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autodesk 3dmax" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "GraphicsCard", "Processor", "ProductAddDate", "ProductBarkod", "ProductBrand", "ProductModel", "ProductName", "Ram", "SerialNumber", "Storage" },
                values: new object[,]
                {
                    { 1, "High-performance laptop", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB054", "", "", "Laptop1", 0, "3872-5930-4832", "" },
                    { 2, "Wireless mouse", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB060", "", "", "Ekipman1", 0, "3840294-9F5A3C2D", "" },
                    { 3, "Mechanical keyboard", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB024", "", "", "Ekipman2", 0, "A2B3-5829-20250111", "" },
                    { 4, "27-inch 4K monitor", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB095", "", "", "Ekipman3", 0, "WLG-384029-2024", "" },
                    { 5, "Noise-cancelling over-ear headphones", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB101", "", "", "Ekipman4", 0, "HDP-230904", "" },
                    { 16, "2TB external hard drive", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB260", "", "", "Desktop1", 0, "EHDD-098723", "" },
                    { 18, "Foldable electric scooter", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB280", "", "", "Desktop2", 0, "ES-129845", "" },
                    { 19, "4K camera drone with flight stabilization", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB295", "", "", "Donanım1", 0, "DRN-589301", "" },
                    { 20, "Portable mini projector", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB310", "", "", "Donanım2", 0, "PRJ-765123", "" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "TeamName" },
                values: new object[,]
                {
                    { 1, "Madbyte" },
                    { 2, "JoyGame" },
                    { 3, "DesertWarrior" },
                    { 4, "Growth" },
                    { 5, "AI" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "1" },
                    { "2", "2" }
                });

            migrationBuilder.InsertData(
                table: "InventoryAssigments",
                columns: new[] { "Id", "AssignmentDate", "PreviusAssigmenId", "ProductId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5073), 3, 1, 1 },
                    { 2, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5075), 3, 2, 2 },
                    { 3, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5076), 3, 3, 1 },
                    { 4, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5078), 3, 4, 2 },
                    { 5, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5083), 3, 5, 3 },
                    { 6, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5085), 3, 16, 4 },
                    { 7, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5086), 3, 18, 5 },
                    { 8, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5087), 3, 19, 6 },
                    { 9, new DateTime(2025, 2, 12, 11, 46, 21, 202, DateTimeKind.Utc).AddTicks(5089), 3, 20, 6 }
                });

            migrationBuilder.InsertData(
                table: "LicenceUser",
                columns: new[] { "Id", "LicenceId", "StaffId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 2, 3 },
                    { 4, 2, 4 },
                    { 5, 3, 5 },
                    { 6, 3, 6 },
                    { 7, 4, 7 },
                    { 8, 4, 8 },
                    { 9, 4, 9 }
                });

            migrationBuilder.InsertData(
                table: "Maintenance",
                columns: new[] { "Id", "CreatedAt", "MaintenanceDescription", "ProductBarkod", "ServiceAdress", "ServiceTitle" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop1", "JGNB054", null, null },
                    { 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman1", "JGNB060", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1, 16 },
                    { 2, 1, 18 },
                    { 3, 2, 1 },
                    { 4, 3, 2 },
                    { 5, 3, 3 },
                    { 6, 3, 4 },
                    { 7, 3, 5 },
                    { 8, 4, 19 },
                    { 9, 4, 20 }
                });

            migrationBuilder.InsertData(
                table: "userTeam",
                columns: new[] { "Id", "StaffId", "TeamId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 2 },
                    { 4, 4, 2 },
                    { 5, 5, 3 },
                    { 6, 6, 3 },
                    { 7, 7, 4 },
                    { 8, 8, 4 },
                    { 9, 9, 5 },
                    { 10, 10, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssigmentHistorys_InventoryAssigmentId",
                table: "AssigmentHistorys",
                column: "InventoryAssigmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssigmentHistorys_ProductId",
                table: "AssigmentHistorys",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AssigmentHistorys_UserId",
                table: "AssigmentHistorys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Url",
                table: "Categories",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAssigments_PreviusAssigmenId",
                table: "InventoryAssigments",
                column: "PreviusAssigmenId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAssigments_ProductId",
                table: "InventoryAssigments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAssigments_UserId",
                table: "InventoryAssigments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Email",
                table: "JoyStaffs",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Licence_LicenceName",
                table: "Licence",
                column: "LicenceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenceUser_LicenceId",
                table: "LicenceUser",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceUser_StaffId",
                table: "LicenceUser",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_ProductBarkod",
                table: "Maintenance",
                column: "ProductBarkod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceHistory_ProductBarkod",
                table: "MaintenanceHistory",
                column: "ProductBarkod");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductId",
                table: "ProductCategories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductBarkod",
                table: "Products",
                column: "ProductBarkod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_TeamName",
                table: "Teams",
                column: "TeamName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userTeam_StaffId",
                table: "userTeam",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_userTeam_TeamId",
                table: "userTeam",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssigmentHistorys");

            migrationBuilder.DropTable(
                name: "LicenceUser");

            migrationBuilder.DropTable(
                name: "Maintenance");

            migrationBuilder.DropTable(
                name: "MaintenanceHistory");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "userTeam");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "InventoryAssigments");

            migrationBuilder.DropTable(
                name: "Licence");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "JoyStaffs");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
