using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoygameInventory.Migrations
{
    /// <inheritdoc />
    public partial class newmig04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "93c69c5e-103d-4d1a-b06e-d4c1542ac9a7", "bd40ed2d-5c71-4235-9339-65ea281c8f81" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6ba7f1e7-55fa-4a3d-b500-4604873f3f0a", "c1bad90e-9ed9-459d-a895-df18de6f079b" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 13, 40, 329, DateTimeKind.Utc).AddTicks(3585));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 13, 40, 329, DateTimeKind.Utc).AddTicks(3588));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "img",
                value: "laptop.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "img",
                value: "mouse.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "img",
                value: "keyboard.jpg");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ProductAddDate", "ProductBarkod", "ProductName", "SerialNumber", "img" },
                values: new object[,]
                {
                    { 4, "27-inch 4K monitor", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB095", "Monitor", "WLG-384029-2024", "monitor.jpg" },
                    { 5, "Noise-cancelling over-ear headphones", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB101", "Headphones", "HDP-230904", "headphones.jpg" },
                    { 6, "128GB USB 3.0 Flash Drive", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "JGNB112", "USB Drive", "USB-3847502", "usbdrive.jpg" }
                });

            migrationBuilder.InsertData(
                table: "InventoryAssigments",
                columns: new[] { "Id", "AssignmentDate", "ProductId", "Status", "UserId" },
                values: new object[,]
                {
                    { 3, new DateTime(2025, 1, 14, 22, 13, 40, 329, DateTimeKind.Utc).AddTicks(3589), 5, "active", "1" },
                    { 4, new DateTime(2025, 1, 14, 22, 13, 40, 329, DateTimeKind.Utc).AddTicks(3590), 6, "active", "1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d1ffd7f1-c057-458e-9788-c61419c20fd3", "bac3af06-a2c2-462e-a2e5-05a8f0ca34b9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f4e6745e-e3c9-47aa-bb74-e88fb721bc10", "593333f8-9ac9-4358-bcbe-7299a63d4f92" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 21, 52, 46, 598, DateTimeKind.Utc).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 21, 52, 46, 598, DateTimeKind.Utc).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "img",
                value: "ürün.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "img",
                value: "ürün.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "img",
                value: "ürün.jpg");
        }
    }
}
