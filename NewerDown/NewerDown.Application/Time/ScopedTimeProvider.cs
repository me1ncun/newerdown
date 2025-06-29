namespace NewerDown.Application.Time;

public class ScopedTimeProvider : IScopedTimeProvider
{
    private readonly TimeProvider _timeProvider;
    
    public ScopedTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }
    
    public DateTime UtcNow()
    {
        return _timeProvider.GetUtcNow().LocalDateTime;
    }
}