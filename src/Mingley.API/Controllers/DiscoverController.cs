using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mingley.Application.DTOs.Common;
using Mingley.Application.DTOs.Discover;
using Mingley.Application.Interfaces;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1")]
[Authorize]
[Produces("application/json")]
public class DiscoverController : ControllerBase
{
    private readonly IDiscoverService _discover;
    public DiscoverController(IDiscoverService discover) => _discover = discover;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get discover feed — matches useUserStore.js loadFeed()</summary>
    [HttpGet("discover")]
    public async Task<IActionResult> GetFeed([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var (users, pagination) = await _discover.GetFeedAsync(CurrentUserId, page, limit);
        return Ok(ApiResponse<object>.Ok(new { users, pagination }));
    }

    /// <summary>Swipe on a profile — like | dislike | superlike</summary>
    [HttpPost("discover/swipe")]
    public async Task<IActionResult> Swipe([FromBody] SwipeRequest req)
    {
        try
        {
            var result = await _discover.SwipeAsync(CurrentUserId, req);
            return Ok(ApiResponse<SwipeResponse>.Ok(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Get all matches — matches MatchesScreen.js and MessagesListScreen.js</summary>
    [HttpGet("matches")]
    public async Task<IActionResult> GetMatches([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var matches = await _discover.GetMatchesAsync(CurrentUserId, page, limit);
        return Ok(ApiResponse<object>.Ok(new { matches }));
    }

    /// <summary>Unmatch with someone</summary>
    [HttpDelete("matches/{matchId}")]
    public async Task<IActionResult> Unmatch(Guid matchId)
    {
        try
        {
            await _discover.UnmatchAsync(CurrentUserId, matchId);
            return Ok(ApiResponse.Ok("Unmatched successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }
}
