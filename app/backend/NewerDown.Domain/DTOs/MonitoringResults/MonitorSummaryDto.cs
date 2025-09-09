namespace NewerDown.Domain.DTOs.MonitoringResults;

public class MonitorSummaryDto
{
    public double UptimePercent { get; set; }
    public double AvgResponseTimeMs { get; set; }
    public int TotalChecks { get; set; }
    public int FailedChecks { get; set; }
    public int IncidentsCount { get; set; }
}