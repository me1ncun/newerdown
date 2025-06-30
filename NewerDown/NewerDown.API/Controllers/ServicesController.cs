using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("/api")]
public class ServicesController : ControllerBase
{
    private readonly IServicesService _service;
    private readonly IFluentValidator _validator;

    public ServicesController(
        IServicesService service,
        IFluentValidator validator)
    {
        _service = service;
        _validator = validator;
    }
    
    [HttpGet("services")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<ServiceDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetServices()
    {
        var services = await _service.GetAllServices();
        
        return Ok(services);
    }
    
    [HttpGet("services/{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ServiceDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetServiceById(Guid id)
    {
        var service = await _service.GetServiceByIdAsync(id);
        
        return Ok(service);
    }
    
    [HttpPost("services")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> CreateService([FromBody] AddServiceDto serviceDto)
    {
        var validationResult = await _validator.ValidateAsync(serviceDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _service.CreateServiceAsync(serviceDto);
        
        return Ok(result);
    }
    
    [HttpPut("services/{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateService(Guid id, [FromBody] UpdateServiceDto serviceDto)
    {
        var validationResult = await _validator.ValidateAsync(serviceDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _service.UpdateServiceAsync(id, serviceDto);
        
        return Ok();
    }
    
    [HttpDelete("services/{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteService(Guid id)
    {
        await _service.DeleteServiceAsync(id);
        
        return Ok();
    }
}