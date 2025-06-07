namespace NewerDown.Domain.DTOs.Service;

public class AddServiceDto
{
    public string Name { get; set; }

    public string Url { get; set; }

    public int CheckIntervalSeconds { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }
}