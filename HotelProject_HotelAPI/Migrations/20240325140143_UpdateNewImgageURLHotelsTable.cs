using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProject_HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewImgageURLHotelsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://placehold.co/600x401");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://placehold.co/600x402");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://placehold.co/600x403");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://placehold.co/600x404");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://placehold.co/600x405");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg");
        }
    }
}
