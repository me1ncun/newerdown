using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewerDown.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedUserInMonitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Monitors_AspNetUsers_UserId",
                table: "Monitors");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Monitors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Monitors_AspNetUsers_UserId",
                table: "Monitors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Monitors_AspNetUsers_UserId",
                table: "Monitors");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Monitors",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Monitors_AspNetUsers_UserId",
                table: "Monitors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
