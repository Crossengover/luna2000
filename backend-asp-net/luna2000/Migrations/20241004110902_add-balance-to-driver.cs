using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace luna2000.Migrations
{
    public partial class addbalancetodriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Drivers");
        }
    }
}
