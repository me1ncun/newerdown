using NewerDown.Domain.Result;

namespace NewerDown.Application.Errors;

public class MonitorErrors
{
    public static readonly Error MonitorNotFound = new Error(
        "Monitors.MonitorNotFound", "Monitor not found");
    
    public static readonly Error TargetNotReachable = new Error(
        "Monitors.TargetNotReachable", "Target URL is not reachable");
}