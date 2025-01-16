using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoygameInventory.Migrations
{
    /// <inheritdoc />
    public partial class Newmig04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "img",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6a47a1c0-684e-468e-ba13-b8010da67f08", "8fc75883-e364-4b09-bd59-f6089b7609be" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cdb5aa55-812c-419b-9225-8987e4226d71", "989843dc-e548-438f-b7bb-04cf068ec4e8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e52542d2-8761-4041-b0e9-e9f693987609", "07e34196-756f-4c34-8c03-49e300b2814e" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9777));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9778));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 3,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 4,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9780));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 5,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 6,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9782));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 7,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 8,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 9,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 45, 52, 565, DateTimeKind.Utc).AddTicks(9785));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5adf5870-7ba3-43a0-9d76-10165c2e207f", "925b425b-5056-457c-8642-c8ca0d9909ae" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a33e9009-b214-4790-b8c9-480476437fc3", "4a751d00-09e5-409a-9a6b-76adab8045f2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7abc19b1-6894-41a8-b9a2-19bf5d601ef1", "afb003b6-a939-496d-90ee-f0200c2073ec" });

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2723));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2726));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 3,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2728));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 4,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2729));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 5,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2730));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 6,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2732));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 7,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2733));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 8,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2734));

            migrationBuilder.UpdateData(
                table: "InventoryAssigments",
                keyColumn: "Id",
                keyValue: 9,
                column: "AssignmentDate",
                value: new DateTime(2025, 1, 16, 10, 21, 42, 924, DateTimeKind.Utc).AddTicks(2736));

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

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "img",
                value: "monitor.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "img",
                value: "headphones.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "img",
                value: "usbdrive.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "img",
                value: "smartphone.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "img",
                value: "tablet.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "img",
                value: "smartwatch.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "img",
                value: "gamingmouse.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "img",
                value: "laptopsleeve.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "img",
                value: "camera.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "img",
                value: "bluetoothspeaker.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "img",
                value: "powerbank.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "img",
                value: "vrheadset.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                column: "img",
                value: "externalharddrive.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "img",
                value: "gamingchair.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "img",
                value: "electricscooter.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "img",
                value: "drone.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "img",
                value: "projector.jpg");
        }
    }
}
