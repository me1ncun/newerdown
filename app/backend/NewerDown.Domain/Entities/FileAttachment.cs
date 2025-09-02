namespace NewerDown.Domain.Entities;

public class FileAttachment
{
    public Guid Id { get; set; }
    
    public string FileName { get; set; }
    
    public string FilePath { get; set; }
    
    public string ContentType { get; set; }
    
    public string Uri { get; set; }
    
    public DateTime CreatedAt { get; set; }
}