using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mingley.Application.DTOs.Common;
using Mingley.Application.DTOs.Subscription;
using Mingley.Application.Interfaces;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/subscriptions")]
[Authorize]
[Produces("application/json")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subs;
    public SubscriptionsController(ISubscriptionService subs) => _subs = subs;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get all plans — matches SubscriptionPlansScreen.js loadPlans()</summary>
    [HttpGet("plans")]
    public async Task<IActionResult> GetPlans()
    {
        var plans = await _subs.GetPlansAsync();
        return Ok(ApiResponse<object>.Ok(new { plans }));
    }

    /// <summary>Get current subscription status</summary>
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var status = await _subs.GetStatusAsync(CurrentUserId);
        return Ok(ApiResponse<UserSubscriptionDto?>.Ok(status));
    }

    /// <summary>Subscribe to a plan — matches SubscriptionPlansScreen.js handleSubscribe()</summary>
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest req)
    {
        try
        {
            var result = await _subs.SubscribeAsync(CurrentUserId, req);
            return Ok(ApiResponse<SubscribeResponse>.Ok(result, "Subscription activated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Cancel subscription</summary>
    [HttpPost("{subscriptionId}/cancel")]
    public async Task<IActionResult> Cancel(Guid subscriptionId, [FromBody] CancelRequest req)
    {
        try
        {
            await _subs.CancelAsync(CurrentUserId, subscriptionId, req.Reason);
            return Ok(ApiResponse.Ok("Subscription cancelled."));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }
}

public class CancelRequest
{
    public string? Reason { get; set; }
}
