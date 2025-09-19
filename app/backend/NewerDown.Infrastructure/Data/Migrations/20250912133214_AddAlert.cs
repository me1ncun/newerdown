using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewerDown.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAlert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentComments_Incidents_IncidentId",
                table: "IncidentComments");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentComments_Incidents_IncidentId",
                table: "IncidentComments",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentComments_Incidents_IncidentId",
                table: "IncidentComments");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentComments_Incidents_IncidentId",
                table: "IncidentComments",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
