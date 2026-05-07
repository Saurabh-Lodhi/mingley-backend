using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Common;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/verify")]
[Authorize]
[Produces("application/json")]
public class VerificationController : ControllerBase
{
    private readonly MingleyDbContext _db;
    public VerificationController(MingleyDbContext db) => _db = db;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Verify profile — awards 50 coins</summary>
    [HttpPost]
    public async Task<IActionResult> Verify([FromBody] VerifyRequest req)
    {
        var user = await _db.Users.FindAsync(CurrentUserId)
            ?? throw new InvalidOperationException("User not found.");

        if (user.IsVerified)
            return BadRequest(ApiResponse<object>.Fail("Profile already verified."));

        user.IsVerified = true;
        user.CoinBalance += MingleyDbContext.VerificationBonus;
        user.UpdatedAt = DateTime.UtcNow;

        _db.CoinTransactions.Add(new CoinTransaction
        {
            UserId = CurrentUserId,
            Coins = MingleyDbContext.VerificationBonus,
            Direction = "credit",
            Description = "Profile verification bonus",
            TransactionType = "verification"
        });

        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(new {
            coinsAwarded = MingleyDbContext.VerificationBonus,
            newBalance = user.CoinBalance,
            message = $"Profile verified! {MingleyDbContext.VerificationBonus} coins added."
        }));
    }
}

public class VerifyRequest { public string? IdProofUrl { get; set; } }
