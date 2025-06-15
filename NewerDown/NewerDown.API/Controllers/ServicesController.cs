using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

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

    [Authorize]
    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var services = await _service.GetAllServices();
        return Ok(services);
    }

    [Authorize]
    [HttpGet("services/{id}")]
    public async Task<IActionResult> GetServiceById(Guid id)
    {
        var service = await _service.GetServiceByIdAsync(id);
        return Ok(service);
    }

    [Authorize]
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
    
    [Authorize]
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
    
    [Authorize]
    [HttpDelete("services/{id}")]
    public async Task<IActionResult> DeleteService(Guid id)
    {
        await _service.DeleteServiceAsync(id);
        return Ok();
    }
}