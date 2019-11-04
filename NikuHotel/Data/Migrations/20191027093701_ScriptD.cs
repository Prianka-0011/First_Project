using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NikuHotel.Data.Migrations
{
    public partial class ScriptD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Bill_BillId",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Payment_BillId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Payment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "Payment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillIssuDate = table.Column<DateTime>(nullable: false),
                    BookingId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Due = table.Column<double>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    PaymentLastDate = table.Column<DateTime>(nullable: false),
                    RoomId = table.Column<int>(nullable: false),
                    TotalBill = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bill_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BillId",
                table: "Payment",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_BookingId",
                table: "Bill",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_CustomerId",
                table: "Bill",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_RoomId",
                table: "Bill",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Bill_BillId",
                table: "Payment",
                column: "BillId",
                principalTable: "Bill",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
