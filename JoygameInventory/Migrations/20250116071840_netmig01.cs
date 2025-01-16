using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoygameInventory.Migrations
{
    /// <inheritdoc />
    public partial class netmig01 : Migration
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
                name: "JoyStaffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
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
                    img = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Depoda")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
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
                    AssignmentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAssigments", x => x.Id);
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
                    { "1", 0, "9c01b5e8-1543-4384-a693-fb01eb44a359", "eren.sezen@joygame.com", false, "Eren", "Sezen", false, null, null, null, null, null, false, "c95f3324-7d84-4cc8-8562-ed9325a6b038", false, "eren_sezen" },
                    { "2", 0, "c55a3ae7-aa99-45d3-8aac-2de3eb3329c8", "osman.benlice@joygame.com", false, "Jane", "Doe", false, null, null, null, null, null, false, "84a9f17b-8c48-4cb4-b1b4-40ca10f2cdfe", false, "osman_benlice" },
                    { "3", 0, "e0f77aef-6a7b-4a48-a485-cd215a53a872", "onur.unlu@joygame.com", false, "Onur", "Ünlü", false, null, null, null, null, null, false, "a5b9ba13-79a3-4661-8c74-ae3f64f631b5", false, "onur.unlu" }
                });

            migrationBuilder.InsertData(
                table: "JoyStaffs",
                columns: new[] { "Id", "Document", "Email", "Name", "PhoneNumber", "Surname", "UserName" },
                values: new object[,]
                {
                    { 1, null, "eren.sezen@joygame.com", "Eren", "555-0101", "Sezen", "eren_sezen" },
                    { 2, null, "osman.benlice@joygame.com", "Osman", "555-0102", "Benlice", "osman_benlice" },
                    { 3, null, "onur.unlu@joygame.com", "Onur", "555-0103", "Ünlü", "onur_unlu" },
                    { 4, null, "ali.karatas@joygame.com", "Ali", "555-0104", "Karataş", "ali_karatas" },
                    { 5, null, "ayse.duran@joygame.com", "Ayşe", "555-0105", "Duran", "ayse_duran" },
                    { 6, null, "fatma.ozdemir@joygame.com", "Fatma", "555-0106", "Özdemir", "fatma_ozdemir" },
                    { 7, null, "mehmet.bayar@joygame.com", "Mehmet", "555-0107", "Bayar", "mehmet_bayar" },
                    { 8, null, "hasan.sahin@joygame.com", "Hasan", "555-0108", "Şahin", "hasan_sahin" },
                    { 9, null, "zeynep.kucuk@joygame.com", "Zeynep", "555-0109", "Küçük", "zeynep_kucuk" },
                    { 10, null, "yusuf.bozkurt@joygame.com", "Yusuf", "555-0110", "Bozkurt", "yusuf_bozkurt" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ProductAddDate", "ProductBarkod", "ProductName", "SerialNumber", "Status", "img" },
                values: new object[,]
                {
                    { 1, "High-performance laptop", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB054", "Laptop", "3872-5930-4832", "Zimmetli", "laptop.jpg" },
                    { 2, "Wireless mouse", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB060", "Mouse", "3840294-9F5A3C2D", "Zimmetli", "mouse.jpg" },
                    { 3, "Mechanical keyboard", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB024", "Keyboard", "A2B3-5829-20250111", "Zimmetli", "keyboard.jpg" },
                    { 4, "27-inch 4K monitor", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB095", "Monitor", "WLG-384029-2024", "Zimmetli", "monitor.jpg" },
                    { 5, "Noise-cancelling over-ear headphones", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB101", "Headphones", "HDP-230904", "Zimmetli", "headphones.jpg" },
                    { 6, "128GB USB 3.0 Flash Drive", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB112", "USB Drive", "USB-3847502", "Zimmetli", "usbdrive.jpg" },
                    { 7, "Latest model smartphone with 5G", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB130", "Smartphone", "SMP-1234A678", "Zimmetli", "smartphone.jpg" },
                    { 8, "10-inch tablet with stylus support", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB145", "Tablet", "TAB-5467D2025", "Zimmetli", "tablet.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ProductAddDate", "ProductBarkod", "ProductName", "SerialNumber", "img" },
                values: new object[,]
                {
                    { 9, "Fitness smartwatch with heart-rate monitor", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB162", "Smartwatch", "SW-9476253", "smartwatch.jpg" },
                    { 10, "High-DPI gaming mouse", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB170", "Gaming Mouse", "GM-845320", "gamingmouse.jpg" },
                    { 11, "Protective laptop sleeve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB183", "Laptop Sleeve", "LS-210987", "laptopsleeve.jpg" },
                    { 12, "DSLR camera with 24MP sensor", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB195", "Camera", "CAM-584230", "camera.jpg" },
                    { 13, "Portable Bluetooth speaker with rich sound", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB210", "Bluetooth Speaker", "BTS-789403", "bluetoothspeaker.jpg" },
                    { 14, "10,000mAh power bank", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB230", "Power Bank", "PB-543210", "powerbank.jpg" },
                    { 15, "Virtual reality headset for immersive experiences", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB245", "VR Headset", "VR-902384", "vrheadset.jpg" },
                    { 16, "2TB external hard drive", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB260", "External Hard Drive", "EHDD-098723", "externalharddrive.jpg" },
                    { 17, "Ergonomic gaming chair", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB275", "Gaming Chair", "GC-765493", "gamingchair.jpg" },
                    { 18, "Foldable electric scooter", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB280", "Electric Scooter", "ES-129845", "electricscooter.jpg" },
                    { 19, "4K camera drone with flight stabilization", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB295", "Drone", "DRN-589301", "drone.jpg" },
                    { 20, "Portable mini projector", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB310", "Projector", "PRJ-765123", "projector.jpg" }
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
                columns: new[] { "Id", "AssignmentDate", "ProductId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3070), 1, 1 },
                    { 2, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3072), 2, 2 },
                    { 3, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3073), 5, 1 },
                    { 4, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3074), 6, 2 },
                    { 5, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3075), 3, 3 },
                    { 6, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3076), 4, 4 },
                    { 7, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3077), 7, 5 },
                    { 8, new DateTime(2025, 1, 16, 7, 18, 40, 187, DateTimeKind.Utc).AddTicks(3078), 8, 6 }
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
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAssigments_ProductId",
                table: "InventoryAssigments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAssigments_UserId",
                table: "InventoryAssigments",
                column: "UserId");
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
                name: "InventoryAssigments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "JoyStaffs");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
