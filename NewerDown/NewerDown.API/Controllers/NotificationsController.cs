using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("/api")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationRuleService _service;
    private readonly IFluentValidator _validator;

    public NotificationsController(
        INotificationRuleService service,
        IFluentValidator validator)
    {
        _service = service;
        _validator = validator;
    }
    
    [HttpGet("notifications/rules")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<NotificationRuleDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetNotificationRules()
    {
        var rules = await _service.GetAllAsync();
        
        return Ok(rules);
    }
    
    [HttpPost("notifications/rules")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> CreateNotificationRule([FromBody] AddNotificationRuleDto ruleDto)
    {
        var validationResult = await _validator.ValidateAsync(ruleDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _service.CreateNotificationRuleAsync(ruleDto);
        
        return Ok();
    }
    
    [HttpDelete("notifications/rules/{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteNotificationRule(Guid id)
    {
        await _service.DeleteNotificationRuleAsync(id);
        
        return Ok();
    }
}