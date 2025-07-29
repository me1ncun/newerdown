using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertController : ControllerBase
{
    private readonly IAlertService _alertService;
    private readonly IFluentValidator _fluentValidator;
    
    public AlertController(IAlertService alertService, IFluentValidator fluentValidator)
    {
        _alertService = alertService;
        _fluentValidator = fluentValidator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAlert([FromBody] AddAlertDto alertDto)
    {
        var validationResult = await _fluentValidator.ValidateAsync(alertDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _alertService.CreateAlertAsync(alertDto);
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var alerts = await _alertService.GetAllAsync();
        
        return Ok(alerts);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var alert = await _alertService.GetAlertByIdAsync(id);
       
        return Ok(alert);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAlert(Guid id, [FromBody] UpdateAlertDto updateAlertDto)
    {
        await _alertService.UpdateAlertAsync(id, updateAlertDto);
        
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAlert([FromBody] Guid id)
    {
        await _alertService.DeleteAlertAsync(id);
        
        return Ok();
    }
    
    [HttpPost("test")]
    public IActionResult SendTest([FromBody] object alertData)
    {
        // Logic to create an alert
        return Ok("Alert created successfully");
    }
}