using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoygameInventory.Migrations
{
    /// <inheritdoc />
    public partial class newmig03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductBarkod",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
                column: "ProductBarkod",
                value: "JGNB054");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductBarkod",
                value: "JGNB060");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductBarkod",
                value: "JGNB024");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductBarkod",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2169176e-9945-4884-8739-8206fe570dd4", "28e992e2-53be-4edb-914a-3982d25543cf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bd8325a2-9630-4112-bd75-54beaf75371e", "1c6f1ac6-8503-4ca4-9eaa-4224e3184c42" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 21, 5, 23, 673, DateTimeKind.Utc).AddTicks(674));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 14, 21, 5, 23, 673, DateTimeKind.Utc).AddTicks(679));
        }
    }
}
