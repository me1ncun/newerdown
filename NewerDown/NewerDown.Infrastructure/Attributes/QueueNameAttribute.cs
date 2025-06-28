namespace NewerDown.Infrastructure.Attributes;

public class QueueNameAttribute : Attribute
{
    public QueueNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}