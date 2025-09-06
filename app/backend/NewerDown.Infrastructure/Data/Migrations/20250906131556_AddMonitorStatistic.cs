using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewerDown.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMonitorStatistic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitorStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonitorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UptimePercent = table.Column<double>(type: "float", nullable: false),
                    AvgResponseTimeMs = table.Column<double>(type: "float", nullable: false),
                    TotalChecks = table.Column<int>(type: "int", nullable: false),
                    FailedChecks = table.Column<int>(type: "int", nullable: false),
                    IncidentsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorStatistics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitorStatistics");
        }
    }
}
