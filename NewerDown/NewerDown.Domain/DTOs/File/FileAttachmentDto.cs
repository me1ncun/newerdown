namespace NewerDown.Domain.DTOs.File;

public class FileAttachmentDto
{
    public string Uri { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string FilePath { get; set; }
    public Stream Content { get; set; }
}