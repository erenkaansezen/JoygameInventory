using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoygameInventory.Migrations
{
    /// <inheritdoc />
    public partial class newmig05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "SecurityStamp", "UserName" },
                values: new object[] { "3cb22093-6c43-44d2-b8b2-2bdca451390c", "eren.sezen@joygame.com", "Eren", "Sezen", "b6917636-6940-4420-b05e-1d19a50d2344", "eren_sezen" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Email", "SecurityStamp", "UserName" },
                values: new object[] { "154cec72-dc7b-487b-98b6-80ea1d476a59", "osman.benlice@joygame.com", "6f6de6d7-a589-4772-a3f1-da5c18c24ac6", "osman_benlice" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3", 0, "e7b0a9a9-b5c9-443c-af80-41325f8791f8", "onur.unlu@joygame.com", false, "Onur", "Ünlü", false, null, null, null, null, null, false, "2a5d9f54-9e14-4dc6-af79-eb9dc51b85c3", false, "onur.unlu" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 16, 10, 661, DateTimeKind.Utc).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 16, 10, 661, DateTimeKind.Utc).AddTicks(9461));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 3,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 16, 10, 661, DateTimeKind.Utc).AddTicks(9462));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 4,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 16, 10, 661, DateTimeKind.Utc).AddTicks(9464));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "SecurityStamp", "UserName" },
                values: new object[] { "93c69c5e-103d-4d1a-b06e-d4c1542ac9a7", "john@example.com", "John", "Doe", "bd40ed2d-5c71-4235-9339-65ea281c8f81", "john_doe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Email", "SecurityStamp", "UserName" },
                values: new object[] { "6ba7f1e7-55fa-4a3d-b500-4604873f3f0a", "jane@example.com", "c1bad90e-9ed9-459d-a895-df18de6f079b", "jane_doe" });

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
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 3,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 13, 40, 329, DateTimeKind.Utc).AddTicks(3589));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 4,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 22, 13, 40, 329, DateTimeKind.Utc).AddTicks(3590));
        }
    }
}
