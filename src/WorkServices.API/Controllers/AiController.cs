using Microsoft.AspNetCore.Mvc;
using WorkServices.API.Contracts.AI;
using WorkServices.Application.Interfaces;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate(
        [FromBody] GeneratePromptRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _aiService.GenerateAsync(
            request.Prompt,
            cancellationToken);

        return Ok(new
        {
            response = result
        });
    }
}