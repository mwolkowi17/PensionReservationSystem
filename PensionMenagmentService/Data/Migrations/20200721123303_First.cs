using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PensionMenagmentService.Data.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Member_since = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestID);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    Is_ocuppied = table.Column<bool>(nullable: false),
                    Smoke = table.Column<bool>(nullable: false),
                    nubmerbeds = table.Column<int>(nullable: false),
                    GuestID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Rooms_Guests_GuestID",
                        column: x => x.GuestID,
                        principalTable: "Guests",
                        principalColumn: "GuestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reserevations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Check_in = table.Column<DateTimeOffset>(nullable: false),
                    Check_out = table.Column<DateTimeOffset>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    GuestID = table.Column<int>(nullable: true),
                    RoomID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserevations", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reserevations_Guests_GuestID",
                        column: x => x.GuestID,
                        principalTable: "Guests",
                        principalColumn: "GuestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserevations_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservationHistoryItems",
                columns: table => new
                {
                    ReservationHistoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    check_in_History = table.Column<DateTimeOffset>(nullable: false),
                    check_out_History = table.Column<DateTimeOffset>(nullable: false),
                    GuestID = table.Column<int>(nullable: true),
                    GuestName_History = table.Column<string>(nullable: true),
                    RoomID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationHistoryItems", x => x.ReservationHistoryID);
                    table.ForeignKey(
                        name: "FK_ReservationHistoryItems_Guests_GuestID",
                        column: x => x.GuestID,
                        principalTable: "Guests",
                        principalColumn: "GuestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservationHistoryItems_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reserevations_GuestID",
                table: "Reserevations",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_Reserevations_RoomID",
                table: "Reserevations",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationHistoryItems_GuestID",
                table: "ReservationHistoryItems",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationHistoryItems_RoomID",
                table: "ReservationHistoryItems",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_GuestID",
                table: "Rooms",
                column: "GuestID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserevations");

            migrationBuilder.DropTable(
                name: "ReservationHistoryItems");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Guests");
        }
    }
}
