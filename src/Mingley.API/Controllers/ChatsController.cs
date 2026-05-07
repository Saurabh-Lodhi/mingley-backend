using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mingley.Application.DTOs.Chat;
using Mingley.Application.DTOs.Common;
using Mingley.Application.Interfaces;
using Mingley.API.Hubs;
using Mingley.Infrastructure.Persistence;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/chats")]
[Authorize]
[Produces("application/json")]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chat;
    private readonly IHubContext<ChatHub> _hub;
    private readonly IHubContext<NotificationHub> _notifHub;
    private readonly MingleyDbContext _db;

    public ChatsController(IChatService chat, IHubContext<ChatHub> hub,
        IHubContext<NotificationHub> notifHub, MingleyDbContext db)
    {
        _chat = chat; _hub = hub; _notifHub = notifHub; _db = db;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetChats()
    {
        var chats = await _chat.GetChatsAsync(CurrentUserId);
        return Ok(ApiResponse<object>.Ok(new { chats }));
    }

    [HttpGet("{chatId}/messages")]
    public async Task<IActionResult> GetMessages(Guid chatId, [FromQuery] int page = 1)
    {
        try
        {
            var messages = await _chat.GetMessagesAsync(CurrentUserId, chatId, page);
            return Ok(ApiResponse<object>.Ok(new { messages }));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }

    [HttpPost("{chatId}/messages")]
    public async Task<IActionResult> SendMessage(Guid chatId, [FromBody] SendMessageRequest req)
    {
        try
        {
            var result = await _chat.SendMessageAsync(CurrentUserId, chatId, req);

            // Push real-time message via SignalR to the chat group
            var sender = await _db.Users.FindAsync(CurrentUserId);
            var msgPayload = new
            {
                id = result.Id,
                senderId = CurrentUserId.ToString(),
                text = req.Text,
                type = req.Type,
                sentAt = DateTime.UtcNow,
                coinsDeducted = result.CoinsDeducted,
                newBalance = result.NewBalance
            };
            await _hub.Clients.Group($"chat_{chatId}").SendAsync("NewMessage", msgPayload);

            // Also send notification to other participant
            var chat = await _db.Chats.Include(c => c.Match)
                .FirstOrDefaultAsync(c => c.Id == chatId, CancellationToken.None);
            if (chat != null)
            {
                var otherId = chat.Match.User1Id == CurrentUserId ? chat.Match.User2Id : chat.Match.User1Id;
                await _notifHub.Clients.Group($"notif_{otherId}").SendAsync("NewMessage", new
                {
                    chatId = chatId.ToString(),
                    senderName = sender?.FullName,
                    senderAvatar = sender?.Avatar,
                    text = req.Text,
                    type = req.Type
                });
            }

            return Ok(ApiResponse<SendMessageResponse>.Ok(result, "Message sent."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPut("{chatId}/read")]
    public async Task<IActionResult> MarkRead(Guid chatId)
    {
        await _chat.MarkReadAsync(CurrentUserId, chatId);
        await _hub.Clients.Group($"chat_{chatId}").SendAsync("MessagesRead", CurrentUserId.ToString());
        return Ok(ApiResponse.Ok("Messages marked as read."));
    }

    [HttpDelete("{chatId}/messages/{messageId}")]
    public async Task<IActionResult> DeleteMessage(Guid chatId, Guid messageId)
    {
        try
        {
            await _chat.DeleteMessageAsync(CurrentUserId, chatId, messageId);
            return Ok(ApiResponse.Ok("Message deleted."));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }

    [HttpGet("{chatId}/quota")]
    public async Task<IActionResult> GetQuota(Guid chatId)
    {
        try
        {
            var quota = await _chat.GetQuotaAsync(CurrentUserId, chatId);
            return Ok(ApiResponse<ChatQuotaDto>.Ok(quota));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }
}