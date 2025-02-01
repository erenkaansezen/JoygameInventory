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
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    ProductBarkod = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ProductAddDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Depoda")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerName = table.Column<string>(type: "TEXT", nullable: true),
                    IPAddress = table.Column<string>(type: "TEXT", nullable: true),
                    MACAddress = table.Column<string>(type: "TEXT", nullable: true),
                    OperatingSystem = table.Column<string>(type: "TEXT", nullable: true),
                    CPU = table.Column<string>(type: "TEXT", nullable: true),
                    RAM = table.Column<int>(type: "INTEGER", nullable: false),
                    Storage = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    DateInstalled = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HostName = table.Column<string>(type: "TEXT", nullable: true),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    NetworkInterface = table.Column<string>(type: "TEXT", nullable: true),
                    PowerStatus = table.Column<string>(type: "TEXT", nullable: true),
                    BackupStatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
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
                name: "UserTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeam_JoyStaffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "JoyStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeam_Teams_TeamId",
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
                    { "1", 0, "09312e9e-0b31-4b86-a953-e22dddf6bb9a", "eren.sezen@joygame.com", false, "Eren", "Sezen", false, null, null, null, null, null, false, "bb886ecc-2abf-4f69-b3d8-215e91a97bf2", false, "eren_sezen" },
                    { "2", 0, "3103bbba-18a4-4b69-b5d1-788359a936d7", "osman.benlice@joygame.com", false, "Jane", "Doe", false, null, null, null, null, null, false, "e8cc008f-1826-4c75-b7de-3db7fa6a624f", false, "osman_benlice" },
                    { "3", 0, "926dd8ae-f8e6-419e-a33d-d713639ff687", "onur.unlu@joygame.com", false, "Onur", "Ünlü", false, null, null, null, null, null, false, "a5da7fc0-f6d8-477e-b392-e23989473594", false, "onur.unlu" }
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
                    { 10, null, "yusuf.bozkurt@joygame.com", "Yusuf", "555-0110", "Bozkurt" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ProductAddDate", "ProductBarkod", "ProductName", "SerialNumber" },
                values: new object[,]
                {
                    { 1, "High-performance laptop", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB054", "Laptop1", "3872-5930-4832" },
                    { 2, "Wireless mouse", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB060", "Ekipman1", "3840294-9F5A3C2D" },
                    { 3, "Mechanical keyboard", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB024", "Ekipman2", "A2B3-5829-20250111" },
                    { 4, "27-inch 4K monitor", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB095", "Ekipman3", "WLG-384029-2024" },
                    { 5, "Noise-cancelling over-ear headphones", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB101", "Ekipman4", "HDP-230904" },
                    { 16, "2TB external hard drive", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB260", "Desktop1", "EHDD-098723" },
                    { 18, "Foldable electric scooter", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB280", "Desktop2", "ES-129845" },
                    { 19, "4K camera drone with flight stabilization", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB295", "Donanım1", "DRN-589301" },
                    { 20, "Portable mini projector", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB310", "Donanım2", "PRJ-765123" }
                });

            migrationBuilder.InsertData(
                table: "Servers",
                columns: new[] { "Id", "BackupStatus", "CPU", "DateInstalled", "HostName", "IPAddress", "Location", "MACAddress", "NetworkInterface", "OperatingSystem", "PowerStatus", "RAM", "SerialNumber", "ServerName", "Status", "Storage" },
                values: new object[,]
                {
                    { 1, "Completed", "Intel Xeon E5-2670", new DateTime(2020, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "server001", "192.168.1.10", "Data Center A", "00:1A:2B:3C:4D:5E", "Ethernet", "Windows Server 2019", "On", 32, "SN123456789", "Server001", "Active", 1024 },
                    { 2, "Pending", "AMD Ryzen 9 5950X", new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "server002", "192.168.1.11", "Data Center B", "00:1A:2B:3C:4D:5F", "WiFi", "Linux Ubuntu 20.04", "Off", 64, "SN987654321", "Server002", "Inactive", 2048 },
                    { 3, "Not Completed", "Intel Core i7-9700K", new DateTime(2022, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "server003", "192.168.1.12", "Data Center C", "00:1A:2B:3C:4D:60", "Ethernet", "Windows Server 2016", "On", 16, "SN246813579", "Server003", "Active", 512 },
                    { 4, "Completed", "Intel Xeon Gold 6248", new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "server004", "192.168.1.13", "Data Center D", "00:1A:2B:3C:4D:61", "Fiber", "Windows Server 2022", "On", 128, "SN654987321", "Server004", "Active", 4096 },
                    { 5, "In Progress", "AMD EPYC 7742", new DateTime(2022, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "server005", "192.168.1.14", "Data Center E", "00:1A:2B:3C:4D:62", "Ethernet", "Linux CentOS 8", "On", 256, "SN9876543210", "Server005", "Active", 8192 },
                    { 6, "Completed", "Intel Core i5-8500", new DateTime(2021, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "server006", "192.168.1.15", "Data Center F", "00:1A:2B:3C:4D:63", "WiFi", "Windows Server 2012", "Off", 8, "SN345678901", "Server006", "Inactive", 256 },
                    { 7, "Completed", "Intel Core i9-9900K", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "server007", "192.168.1.16", "Data Center G", "00:1A:2B:3C:4D:64", "Ethernet", "Linux Debian 10", "On", 64, "SN789456123", "Server007", "Active", 2048 },
                    { 8, "Pending", "Intel Xeon E3-1230", new DateTime(2020, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "server008", "192.168.1.17", "Data Center H", "00:1A:2B:3C:4D:65", "WiFi", "Windows Server 2016", "Off", 16, "SN963852741", "Server008", "Inactive", 512 },
                    { 9, "Not Completed", "AMD Ryzen 5 3600", new DateTime(2022, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "server009", "192.168.1.18", "Data Center I", "00:1A:2B:3C:4D:66", "Ethernet", "Linux Ubuntu 18.04", "On", 16, "SN852741963", "Server009", "Active", 1024 },
                    { 10, "Completed", "Intel Core i7-10700K", new DateTime(2021, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "server010", "192.168.1.19", "Data Center J", "00:1A:2B:3C:4D:67", "Ethernet", "Windows Server 2019", "On", 32, "SN1029384756", "Server010", "Active", 2048 }
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
                    { 1, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9392), 3, 1, 1 },
                    { 2, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9395), 3, 2, 2 },
                    { 3, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9396), 3, 3, 1 },
                    { 4, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9397), 3, 4, 2 },
                    { 5, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9399), 3, 5, 3 },
                    { 6, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9400), 3, 16, 4 },
                    { 7, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9401), 3, 18, 5 },
                    { 8, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9402), 3, 19, 6 },
                    { 9, new DateTime(2025, 1, 31, 23, 57, 28, 571, DateTimeKind.Utc).AddTicks(9403), 3, 20, 6 }
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
                table: "UserTeam",
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
                name: "IX_UserTeam_StaffId",
                table: "UserTeam",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeam_TeamId",
                table: "UserTeam",
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
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "UserTeam");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "InventoryAssigments");

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
