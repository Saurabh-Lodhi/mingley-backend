using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Mingley.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> _connections = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            _connections[userId] = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null) _connections.Remove(userId);
        await base.OnDisconnectedAsync(exception);
    }

    // Join a chat room
    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"chat_{chatId}");
    }

    // Leave a chat room
    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat_{chatId}");
    }

    // Typing indicator
    public async Task Typing(string chatId, bool isTyping)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        await Clients.OthersInGroup($"chat_{chatId}").SendAsync("UserTyping", userId, isTyping);
    }

    // Get connection ID for a user (for calling)
    public static string? GetConnectionId(string userId) =>
        _connections.TryGetValue(userId, out var cid) ? cid : null;
}
