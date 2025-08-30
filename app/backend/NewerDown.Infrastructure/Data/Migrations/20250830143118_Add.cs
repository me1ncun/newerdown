using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewerDown.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionExpiresAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlan",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionExpiresAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlan",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "AspNetUsers");
        }
    }
}
