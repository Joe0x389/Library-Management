using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AlignFinesPaymentsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Fines");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Payments",
                newName: "PaidAt");

            migrationBuilder.RenameColumn(
                name: "IssuedDate",
                table: "Fines",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Fines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Fines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaidAt",
                table: "Payments",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Fines",
                newName: "IssuedDate");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Fines",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "Fines",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Fines",
                type: "datetime2",
                nullable: true);
        }
    }
}
