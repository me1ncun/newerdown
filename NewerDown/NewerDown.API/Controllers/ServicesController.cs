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
    private readonly ILogger<ServicesController> _logger;
    private readonly IServicesService _service;
    private readonly IFluentValidator _validator;

    public ServicesController(
        ILogger<ServicesController> logger,
        IServicesService service,
        IFluentValidator validator)
    {
        _logger = logger;
        _service = service;
        _validator = validator;
    }

    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var services = await _service.GetAllServices();
        return Ok(services);
    }

    [HttpGet("services/{id}")]
    public async Task<IActionResult> GetServiceById(Guid id)
    {
        var service = await _service.GetServiceByIdAsync(id);
        return Ok(service);
    }

    [HttpPost("services")]
    public async Task<IActionResult> CreateService([FromBody] AddServiceDto serviceDto)
    {
        var validationResult = await _validator.ValidateAsync(serviceDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _service.CreateServiceAsync(serviceDto);
        return Ok();
    }
    
    [HttpPut("services/{id}")]
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
    public async Task<IActionResult> DeleteService(Guid id)
    {
        await _service.DeleteServiceAsync(id);
        return Ok();
    }
}