using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewerDown.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FileAttachments_FileAttachmentId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileAttachmentId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FileAttachments_FileAttachmentId",
                table: "AspNetUsers",
                column: "FileAttachmentId",
                principalTable: "FileAttachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FileAttachments_FileAttachmentId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileAttachmentId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FileAttachments_FileAttachmentId",
                table: "AspNetUsers",
                column: "FileAttachmentId",
                principalTable: "FileAttachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
