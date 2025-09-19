using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.DTOs.Request;
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
    public async Task<IActionResult> CreateAlert([FromBody] AddAlertDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _alertService.CreateAlertAsync(request);
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var alerts = await _alertService.GetAllAsync();
        
        return Ok(alerts);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetById(GetByIdDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var alert = await _alertService.GetAlertByIdAsync(request);
        
        return Ok(alert);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAlert(Guid id, [FromBody] UpdateAlertDto request)
    {
        if(id == Guid.Empty)
            return BadRequest("Id cannot be empty");
        
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _alertService.UpdateAlertAsync(id, request);
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAlert([FromBody] DeleteAlertDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _alertService.DeleteAlertAsync(request);
        
        return Ok();
    }
}