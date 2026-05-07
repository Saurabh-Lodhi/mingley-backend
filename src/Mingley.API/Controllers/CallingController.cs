using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Common;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;
using Mingley.API.Hubs;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/calls")]
[Authorize]
[Produces("application/json")]
public class CallingController : ControllerBase
{
    private readonly MingleyDbContext _db;
    private readonly IHubContext<ChatHub> _hub;

    public CallingController(MingleyDbContext db, IHubContext<ChatHub> hub)
    {
        _db = db;
        _hub = hub;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Initiate a call — notifies receiver via SignalR. Costs: audio 10/min, video 100/min</summary>
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromBody] InitiateCallRequest req)
    {
        if (!Guid.TryParse(req.TargetId, out var targetId))
            return BadRequest(ApiResponse<object>.Fail("Invalid target ID."));

        var caller = await _db.Users.FindAsync(CurrentUserId)
            ?? throw new InvalidOperationException("User not found.");

        // Check they are matched
        var matched = await _db.Matches.AnyAsync(m =>
            m.IsActive && !m.IsDeleted &&
            ((m.User1Id == CurrentUserId && m.User2Id == targetId) ||
             (m.User1Id == targetId && m.User2Id == CurrentUserId)));

        if (!matched)
            return BadRequest(ApiResponse<object>.Fail("You can only call your matches."));

        var session = new CallSession
        {
            CallerId   = CurrentUserId,
            ReceiverId = targetId,
            CallType   = req.CallType ?? "audio",
            Status     = "ringing"
        };
        _db.CallSessions.Add(session);
        await _db.SaveChangesAsync();

        // Notify receiver via SignalR
        await _hub.Clients.Group($"user_{targetId}").SendAsync("IncomingCall", new
        {
            callId   = session.Id.ToString(),
            callType = session.CallType,
            caller   = new { id = caller.Id.ToString(), name = caller.FullName, avatar = caller.Avatar }
        });

        return Ok(ApiResponse<object>.Ok(new {
            callId   = session.Id.ToString(),
            callType = session.CallType,
            status   = session.Status,
            costPerMin = session.CallType == "video"
                ? MingleyDbContext.VideoCallCoinPerMin
                : MingleyDbContext.AudioCallCoinPerMin
        }));
    }

    /// <summary>Answer a call</summary>
    [HttpPost("{callId}/answer")]
    public async Task<IActionResult> Answer(Guid callId)
    {
        var session = await _db.CallSessions.FindAsync(callId)
            ?? throw new InvalidOperationException("Call not found.");

        if (session.ReceiverId != CurrentUserId)
            return Forbid();

        session.Status     = "active";
        session.AnsweredAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        await _hub.Clients.Group($"user_{session.CallerId}").SendAsync("CallAnswered", new { callId = callId.ToString() });

        return Ok(ApiResponse<object>.Ok(new { callId = callId.ToString(), status = "active" }));
    }

    /// <summary>End a call — calculates and deducts coins</summary>
    [HttpPost("{callId}/end")]
    public async Task<IActionResult> End(Guid callId, [FromBody] EndCallRequest req)
    {
        var session = await _db.CallSessions
            .Include(c => c.Caller)
            .FirstOrDefaultAsync(c => c.Id == callId)
            ?? throw new InvalidOperationException("Call not found.");

        session.Status    = "ended";
        session.EndedAt   = DateTime.UtcNow;
        session.EndReason = req.Reason ?? "user_ended";

        int coinsDeducted = 0;
        if (session.AnsweredAt.HasValue)
        {
            var durationSeconds = (int)(DateTime.UtcNow - session.AnsweredAt.Value).TotalSeconds;
            session.DurationSeconds = durationSeconds;

            var durationMinutes = (int)Math.Ceiling(durationSeconds / 60.0);
            var ratePerMin = session.CallType == "video"
                ? MingleyDbContext.VideoCallCoinPerMin
                : MingleyDbContext.AudioCallCoinPerMin;

            coinsDeducted = durationMinutes * ratePerMin;

            // Deduct from caller
            if (session.Caller != null && session.Caller.CoinBalance >= coinsDeducted)
            {
                session.Caller.CoinBalance -= coinsDeducted;
                session.CoinsDeducted = coinsDeducted;
                _db.CoinTransactions.Add(new CoinTransaction
                {
                    UserId = session.CallerId,
                    Coins  = coinsDeducted,
                    Direction = "debit",
                    Description = $"{session.CallType} call ({durationMinutes} min)",
                    TransactionType = "call"
                });
            }
        }

        await _db.SaveChangesAsync();

        // Notify other party
        var otherId = session.CallerId == CurrentUserId ? session.ReceiverId : session.CallerId;
        await _hub.Clients.Group($"user_{otherId}").SendAsync("CallEnded", new { callId = callId.ToString() });

        return Ok(ApiResponse<object>.Ok(new {
            callId          = callId.ToString(),
            duration        = session.DurationSeconds,
            coinsDeducted,
            newBalance      = session.Caller?.CoinBalance
        }));
    }

    /// <summary>Decline a call</summary>
    [HttpPost("{callId}/decline")]
    public async Task<IActionResult> Decline(Guid callId)
    {
        var session = await _db.CallSessions.FindAsync(callId)
            ?? throw new InvalidOperationException("Call not found.");
        session.Status  = "declined";
        session.EndedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        await _hub.Clients.Group($"user_{session.CallerId}").SendAsync("CallDeclined", new { callId = callId.ToString() });

        return Ok(ApiResponse.Ok("Call declined."));
    }

    /// <summary>Get call history</summary>
    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        var calls = await _db.CallSessions
            .Include(c => c.Caller)
            .Include(c => c.Receiver)
            .Where(c => c.CallerId == CurrentUserId || c.ReceiverId == CurrentUserId)
            .OrderByDescending(c => c.CreatedAt)
            .Take(50)
            .Select(c => new {
                id = c.Id.ToString(),
                c.CallType, c.Status, c.DurationSeconds, c.CoinsDeducted,
                c.CreatedAt,
                caller   = new { id = c.Caller!.Id.ToString(),   name = c.Caller.FullName,   avatar = c.Caller.Avatar },
                receiver = new { id = c.Receiver!.Id.ToString(),  name = c.Receiver.FullName, avatar = c.Receiver.Avatar }
            }).ToListAsync();

        return Ok(ApiResponse<object>.Ok(new { calls }));
    }
}

public class InitiateCallRequest { public string? TargetId { get; set; } public string? CallType { get; set; } = "audio"; }
public class EndCallRequest      { public string? Reason   { get; set; } }
