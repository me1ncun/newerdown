using GraphQL;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.DTOs.Request;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

/// <summary>
/// Provides API endpoints for managing alert configurations and notifications.
/// </summary>
[ApiController]
[Authorize]
[Route("api/alerts")]
public class AlertController : ControllerBase
{
    private readonly IAlertService _alertService;
    private readonly IFluentValidator _fluentValidator;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AlertController"/> class.
    /// </summary>
    /// <param name="alertService">The service responsible for handling alert operations.</param>
    /// <param name="fluentValidator">The validator used to validate request DTOs.</param>
    public AlertController(IAlertService alertService, IFluentValidator fluentValidator)
    {
        _alertService = alertService;
        _fluentValidator = fluentValidator;
    }
    
    /// <summary>
    /// Creates a new alert configuration.
    /// </summary>
    /// <param name="request">The alert data to create.</param>
    /// <returns>Returns a success response when the alert is created.</returns>
    /// <response code="200">Alert successfully created.</response>
    /// <response code="400">Validation failed or request is invalid.</response>
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
    
    /// <summary>
    /// Retrieves all alerts.
    /// </summary>
    /// <returns>Returns a list of all configured alerts.</returns>
    /// <response code="200">Successfully retrieved alerts.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var alerts = await _alertService.GetAllAsync();
        
        return Ok(alerts);
    }
    
    /// <summary>
    /// Retrieves a specific alert by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the alert.</param>
    /// <returns>Returns the alert details.</returns>
    /// <response code="200">Alert successfully retrieved.</response>
    /// <response code="400">Invalid or missing ID.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if(id == Guid.Empty)
            return BadRequest("Id cannot be empty");
        
        var alert = await _alertService.GetAlertByIdAsync(id);
        
        return Ok(alert);
    }
    
    /// <summary>
    /// Updates an existing alert.
    /// </summary>
    /// <param name="id">The unique identifier of the alert to update.</param>
    /// <param name="request">The updated alert data.</param>
    /// <returns>Returns a success response when the alert is updated.</returns>
    /// <response code="200">Alert successfully updated.</response>
    /// <response code="400">Invalid input or validation failed.</response>
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
    
    /// <summary>
    /// Deletes an existing alert.
    /// </summary>
    /// <param name="request">The request containing information about which alert to delete.</param>
    /// <returns>Returns a success response when the alert is deleted.</returns>
    /// <response code="200">Alert successfully deleted.</response>
    /// <response code="400">Validation failed or request is invalid.</response>
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