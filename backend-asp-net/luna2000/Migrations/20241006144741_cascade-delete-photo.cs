using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace luna2000.Migrations
{
    public partial class cascadedeletephoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Cars_CarId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Drivers_DriverId",
                table: "Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Cars_CarId",
                table: "Photos",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Drivers_DriverId",
                table: "Photos",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Cars_CarId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Drivers_DriverId",
                table: "Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Cars_CarId",
                table: "Photos",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Drivers_DriverId",
                table: "Photos",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }
    }
}
