using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.File;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("/api")]
public class FileAttachmentController : ControllerBase
{
    private readonly IBlobStorageService _service;

    public FileAttachmentController(IBlobStorageService service)
    {
        _service = service;
    }
    
    [HttpPost("fileattachment/upload")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(FileAttachmentResponseDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var response = await _service.UploadFileAsync(file);
        
        return Ok(response);
    }
}