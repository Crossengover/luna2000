using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace luna2000.Migrations
{
    public partial class changepk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Cars_CarId",
                table: "CarRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Drivers_DriverId",
                table: "CarRentals");

            migrationBuilder.DropIndex(
                name: "IX_CarRentals_CarId",
                table: "CarRentals");

            migrationBuilder.DropIndex(
                name: "IX_CarRentals_DriverId",
                table: "CarRentals");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Photos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "CarId1",
                table: "CarRentals",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId1",
                table: "CarRentals",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CarRentals_CarId1",
                table: "CarRentals",
                column: "CarId1");

            migrationBuilder.CreateIndex(
                name: "IX_CarRentals_DriverId1",
                table: "CarRentals",
                column: "DriverId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Cars_CarId1",
                table: "CarRentals",
                column: "CarId1",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Drivers_DriverId1",
                table: "CarRentals",
                column: "DriverId1",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Cars_CarId1",
                table: "CarRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Drivers_DriverId1",
                table: "CarRentals");

            migrationBuilder.DropIndex(
                name: "IX_CarRentals_CarId1",
                table: "CarRentals");

            migrationBuilder.DropIndex(
                name: "IX_CarRentals_DriverId1",
                table: "CarRentals");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarRentals");

            migrationBuilder.DropColumn(
                name: "DriverId1",
                table: "CarRentals");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Photos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_CarRentals_CarId",
                table: "CarRentals",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRentals_DriverId",
                table: "CarRentals",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Cars_CarId",
                table: "CarRentals",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Drivers_DriverId",
                table: "CarRentals",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
