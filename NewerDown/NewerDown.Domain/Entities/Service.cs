using System.ComponentModel.DataAnnotations.Schema;

namespace NewerDown.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Url { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}