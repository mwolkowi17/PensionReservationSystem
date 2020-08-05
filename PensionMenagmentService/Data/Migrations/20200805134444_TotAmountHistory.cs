using Microsoft.EntityFrameworkCore.Migrations;

namespace PensionMenagmentService.Data.Migrations
{
    public partial class TotAmountHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAmount_History",
                table: "ReservationHistoryItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount_History",
                table: "ReservationHistoryItems");
        }
    }
}
