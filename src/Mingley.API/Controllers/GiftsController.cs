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
[Route("v1/gifts")]
[Authorize]
[Produces("application/json")]
public class GiftsController : ControllerBase
{
    private readonly MingleyDbContext _db;
    private readonly IHubContext<ChatHub> _hub;

    public GiftsController(MingleyDbContext db, IHubContext<ChatHub> hub)
    {
        _db = db; _hub = hub;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get gift catalog — all gifts with coin costs</summary>
    [HttpGet("catalog")]
    public async Task<IActionResult> GetCatalog()
    {
        var gifts = await _db.Gifts.Where(g => g.IsActive && !g.IsDeleted)
            .Select(g => new { id = g.Id.ToString(), g.Name, g.Icon, g.CoinCost })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(new { gifts }));
    }

    /// <summary>Send a gift — deducts coins and sends notification</summary>
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendGiftRequest req)
    {
        if (!Guid.TryParse(req.GiftId, out var giftId) || !Guid.TryParse(req.RecipientId, out var recipientId))
            return BadRequest(ApiResponse<object>.Fail("Invalid IDs."));

        var sender = await _db.Users.FindAsync(CurrentUserId)
            ?? throw new InvalidOperationException("Sender not found.");
        var gift = await _db.Gifts.FindAsync(giftId)
            ?? throw new InvalidOperationException("Gift not found.");

        if (sender.CoinBalance < gift.CoinCost)
            return BadRequest(ApiResponse<object>.Fail($"Insufficient coins. Need {gift.CoinCost}, have {sender.CoinBalance}."));

        sender.CoinBalance -= gift.CoinCost;
        _db.CoinTransactions.Add(new CoinTransaction
        {
            UserId = CurrentUserId, Coins = gift.CoinCost, Direction = "debit",
            Description = $"Gift sent: {gift.Name}", TransactionType = "gift"
        });

        // If chatId provided, add gift message to chat
        if (!string.IsNullOrEmpty(req.ChatId) && Guid.TryParse(req.ChatId, out var chatId))
        {
            var msg = new Message
            {
                ChatId = chatId, SenderId = CurrentUserId,
                Type = "gift", GiftName = gift.Name, GiftCost = gift.CoinCost,
                Text = req.Message ?? $"Sent a {gift.Name} 🎁"
            };
            _db.Messages.Add(msg);
            await _db.SaveChangesAsync();

            // Push to chat room via SignalR
            await _hub.Clients.Group($"chat_{chatId}").SendAsync("NewMessage", new
            {
                id = msg.Id.ToString(), senderId = CurrentUserId.ToString(),
                type = "gift", giftName = gift.Name, giftCost = gift.CoinCost,
                text = msg.Text, sentAt = msg.CreatedAt
            });
        }
        else
        {
            await _db.SaveChangesAsync();
        }

        // Send notification to recipient
        var notification = new Notification
        {
            UserId = recipientId, Title = "You received a gift! 🎁",
            Body = $"{sender.FullName} sent you a {gift.Name}",
            Type = "gift"
        };
        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();

        await _hub.Clients.Group($"user_{recipientId}").SendAsync("NewNotification", new
        {
            id = notification.Id.ToString(), notification.Title, notification.Body, notification.Type
        });

        return Ok(ApiResponse<object>.Ok(new { newBalance = sender.CoinBalance }));
    }
}

public class SendGiftRequest
{
    public string? RecipientId { get; set; }
    public string? GiftId      { get; set; }
    public string? ChatId      { get; set; }
    public string? Message     { get; set; }
}
