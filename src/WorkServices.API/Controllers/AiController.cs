using Microsoft.AspNetCore.Mvc;
using WorkServices.API.Contracts.AI;
using WorkServices.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;


namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;
    private readonly ILogger<AiController> _logger;

    public AiController(IAiService aiService, 
                         ILogger<AiController> logger)
    {
        _aiService = aiService;
         _logger = logger;
    }

   [Authorize(Policy = "ArtisanOnly")]
   [Authorize(Policy = "CustomerOnly")]
   [EnableRateLimiting("AiPolicy")]
   [HttpPost("generate")]
  public async Task<IActionResult> Generate(
    [FromBody] GeneratePromptRequest request,
    CancellationToken cancellationToken)
{
    if (string.IsNullOrWhiteSpace(request.Prompt))
    {
        return BadRequest("Prompt is required.");
    }
     if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
       var userId =
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

   _logger.LogInformation(
    "User {UserId} generated AI request",
    userId);

    var result = await _aiService.GenerateAsync(
        request.Prompt,
        cancellationToken);

    return Ok(new
    {
        response = result
    });
}
}