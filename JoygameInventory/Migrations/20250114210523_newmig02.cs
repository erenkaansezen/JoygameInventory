using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoygameInventory.Migrations
{
    /// <inheritdoc />
    public partial class newmig02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "InventoryAssigments");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "InventoryAssigments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "AssignmentDate", "Status" },
                values: new object[] { new DateTime(2025, 1, 14, 21, 5, 23, 673, DateTimeKind.Utc).AddTicks(674), "active" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssignmentDate", "Status" },
                values: new object[] { new DateTime(2025, 1, 14, 21, 5, 23, 673, DateTimeKind.Utc).AddTicks(679), "active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "InventoryAssigments");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                table: "InventoryAssigments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8825a24f-4e86-4514-85d7-44976da8dc16", "6c505d85-421f-4d47-8d6a-369c9d7cd76d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "58dd568d-4666-421b-80ed-6da100c4d21f", "7a9d0f67-efc8-40c9-8087-d9e1f545d4c2" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AssignmentDate", "ReturnedDate" },
                values: new object[] { new DateTime(2025, 1, 14, 20, 47, 12, 949, DateTimeKind.Utc).AddTicks(7942), null });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssignmentDate", "ReturnedDate" },
                values: new object[] { new DateTime(2025, 1, 14, 20, 47, 12, 949, DateTimeKind.Utc).AddTicks(7946), null });
        }
    }
}
