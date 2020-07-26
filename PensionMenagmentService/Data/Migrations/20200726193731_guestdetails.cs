using Microsoft.EntityFrameworkCore.Migrations;

namespace PensionMenagmentService.Data.Migrations
{
    public partial class guestdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Guests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAdress",
                table: "Guests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Guests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreetNumber",
                table: "Guests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TelephoneNumber",
                table: "Guests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "EmailAdress",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "TelephoneNumber",
                table: "Guests");
        }
    }
}
