using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewerDown.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "MonitorChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StatusCode",
                table: "MonitorChecks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Alerts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Alerts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "MonitorChecks");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "MonitorChecks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Alerts");
        }
    }
}
