using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/cache")]
public class CacheController : ControllerBase
{
    private readonly IDistributedCache _cache;

    public CacheController(IDistributedCache cache)
    {
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        await _cache.SetStringAsync("hello", "world");

        var value = await _cache.GetStringAsync("hello");

        return Ok(value);
    }
}