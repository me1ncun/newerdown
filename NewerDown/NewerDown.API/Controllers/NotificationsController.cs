using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

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

    [Authorize]
    [HttpGet("notifications/rules")]
    public async Task<IActionResult> GetNotificationRules()
    {
        var rules = await _service.GetAllAsync();
        return Ok(rules);
    }

    [Authorize]
    [HttpPost("notifications/rules")]
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

    [Authorize]
    [HttpDelete("notifications/rules/{id}")]
    public async Task<IActionResult> DeleteNotificationRule(Guid id)
    {
        await _service.DeleteNotificationRuleAsync(id);
        return Ok();
    }
}