namespace NewerDown.Application.Time;

public interface IScopedTimeProvider
{
    DateTime UtcNow();
}