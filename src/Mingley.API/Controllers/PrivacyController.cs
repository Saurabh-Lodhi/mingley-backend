using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Common;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/privacy")]
[Authorize]
[Produces("application/json")]
public class PrivacyController : ControllerBase
{
    private readonly MingleyDbContext _db;
    public PrivacyController(MingleyDbContext db) => _db = db;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get privacy policy text</summary>
    [HttpGet("policy")]
    [AllowAnonymous]
    public IActionResult GetPolicy()
    {
        return Ok(ApiResponse<object>.Ok(new {
            title = "Mingley Privacy Policy & Match Agreement",
            content = @"By using Mingley, you agree to the following:

1. SAFETY & RESPONSIBILITY
   - Do not share personal phone numbers or social media handles in chat
   - Any monetary transactions arranged outside the app are at your own risk
   - Mingley is not responsible for offline interactions

2. COIN ECONOMY
   - Minimum coin purchase: ₹1,000 for 1,000 coins
   - Audio calls: 10 coins/minute
   - Video calls: 100 coins/minute
   - Gifts: Heart (10), Rose (20), Gift (50), Coffee Date (200)
   - Female users can withdraw 70% of total coin earnings
   - 50 coins bonus on profile verification

3. PROHIBITED CONTENT
   - No exchange of personal contact information in chat
   - No harassment, abuse or inappropriate content
   - Violations may result in account suspension

4. MATCH PRIVACY
   - Your location is approximate only
   - Profile information is shared only with your matches

By continuing, you accept these terms.",
            lastUpdated = "2024-01-01"
        }));
    }

    /// <summary>Accept privacy popup after match</summary>
    [HttpPost("accept/{matchId}")]
    public async Task<IActionResult> Accept(Guid matchId)
    {
        var existing = await _db.PrivacyAgreements
            .AnyAsync(p => p.UserId == CurrentUserId && p.MatchId == matchId);

        if (!existing)
        {
            _db.PrivacyAgreements.Add(new PrivacyAgreement
            {
                UserId = CurrentUserId, MatchId = matchId, Accepted = true
            });
            await _db.SaveChangesAsync();
        }
        return Ok(ApiResponse.Ok("Privacy policy accepted."));
    }
}
