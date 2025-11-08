using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewerDown.Controllers;

[ApiController]
[Authorize]
[Route("api/integrations")]
public class IntegrationController : ControllerBase
{
    [HttpPost("email")]
    public IActionResult CreateEmailIntegration([FromBody] object integrationData)
    {
        // Logic to create an integration
        return Ok("Integration created successfully");
    }
    
    [HttpGet]
    public IActionResult GetAll([FromBody] object integrationData)
    {
        // Logic to create an integration
        return Ok("Integration created successfully");
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeactivateIntegration([FromBody] object integrationData)
    {
        // Logic to create an integration
        return Ok("Integration created successfully");
    }
}