using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NikuHotel.Data.Migrations
{
    public partial class ScriptC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Employee_EmployeeId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_EmployeeId",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Bill");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Bill",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_EmployeeId",
                table: "Bill",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Employee_EmployeeId",
                table: "Bill",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
