using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace luna2000.Migrations
{
    public partial class lognote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "BaseLogs",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "BaseLogs");
        }
    }
}
