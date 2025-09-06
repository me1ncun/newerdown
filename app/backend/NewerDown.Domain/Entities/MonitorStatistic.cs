namespace NewerDown.Domain.Entities;

public class MonitorStatistic
{
    public Guid Id { get; set; }
    public Guid MonitorId { get; set; }

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    public double UptimePercent { get; set; }
    public double AvgResponseTimeMs { get; set; }
    public int TotalChecks { get; set; }
    public int FailedChecks { get; set; }
    public int IncidentsCount { get; set; }
}