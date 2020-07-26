using Microsoft.EntityFrameworkCore.Migrations;

namespace PensionMenagmentService.Data.Migrations
{
    public partial class adresscorrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "Guests");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Guests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Guests");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreetNumber",
                table: "Guests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
