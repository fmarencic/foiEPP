using Microsoft.EntityFrameworkCore.Migrations;

namespace foiEPP.Migrations
{
    public partial class EncodingTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Encoding",
                table: "UserImages");

            migrationBuilder.AddColumn<string>(
                name: "InternalData",
                table: "UserImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalData",
                table: "UserImages");

            migrationBuilder.AddColumn<string>(
                name: "Encoding",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
