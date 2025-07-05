namespace NewerDown.Domain.DTOs.File;

public class FileAttachmentResponseDto
{
    public string Status { get; set; }
    public bool Error { get; set; }
    public FileAttachmentDto FileAttachment { get; set; }
}