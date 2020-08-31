using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace foiEPP.Migrations
{
    public partial class EncodingChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalData",
                table: "UserImages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Encoding",
                table: "UserImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Encoding",
                table: "UserImages");

            migrationBuilder.AddColumn<string>(
                name: "InternalData",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
