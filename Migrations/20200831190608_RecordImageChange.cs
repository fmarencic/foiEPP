using Microsoft.EntityFrameworkCore.Migrations;

namespace foiEPP.Migrations
{
    public partial class RecordImageChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Encoding",
                table: "Records");

            migrationBuilder.AddColumn<byte>(
                name: "Image",
                table: "Records",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Records");

            migrationBuilder.AddColumn<string>(
                name: "Encoding",
                table: "Records",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
