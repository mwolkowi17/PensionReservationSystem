using Microsoft.EntityFrameworkCore.Migrations;

namespace PensionMenagmentService.Data.Migrations
{
    public partial class RegularPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReguralPrice",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReguralPrice",
                table: "Rooms");
        }
    }
}
