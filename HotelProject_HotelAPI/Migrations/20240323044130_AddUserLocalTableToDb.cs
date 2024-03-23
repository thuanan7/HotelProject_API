using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProject_HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLocalTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUser", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 23, 11, 41, 29, 239, DateTimeKind.Local).AddTicks(6560));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 23, 11, 41, 29, 239, DateTimeKind.Local).AddTicks(6574));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 23, 11, 41, 29, 239, DateTimeKind.Local).AddTicks(6575));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 23, 11, 41, 29, 239, DateTimeKind.Local).AddTicks(6577));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 23, 11, 41, 29, 239, DateTimeKind.Local).AddTicks(6578));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUser");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 21, 11, 0, 10, 279, DateTimeKind.Local).AddTicks(5043));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 21, 11, 0, 10, 279, DateTimeKind.Local).AddTicks(5056));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 21, 11, 0, 10, 279, DateTimeKind.Local).AddTicks(5058));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 21, 11, 0, 10, 279, DateTimeKind.Local).AddTicks(5060));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 21, 11, 0, 10, 279, DateTimeKind.Local).AddTicks(5061));
        }
    }
}
