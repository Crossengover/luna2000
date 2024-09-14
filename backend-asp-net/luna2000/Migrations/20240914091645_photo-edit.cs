using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace luna2000.Migrations
{
    public partial class photoedit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Photos",
                newName: "FileName");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Photos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Photos",
                newName: "Path");
        }
    }
}
