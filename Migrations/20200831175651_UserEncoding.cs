using Microsoft.EntityFrameworkCore.Migrations;

namespace foiEPP.Migrations
{
    public partial class UserEncoding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstNameEncoded",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNameEncoded",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNameEncoded",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastNameEncoded",
                table: "Users");
        }
    }
}
