using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Chat;
using Mingley.Application.Interfaces;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;

namespace Mingley.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly MingleyDbContext _db;
    private readonly IWalletService _wallet;

    private const int MaleCostPerMessage        = 10;
    private const int MalePremiumCostPerMessage = 5;
    private const int FemaleFreeMessages        = 3;
    private const int FemaleMessageCost         = 5;

    public ChatService(MingleyDbContext db, IWalletService wallet)
    {
        _db = db; _wallet = wallet;
    }

    public async Task<List<ChatListItemDto>> GetChatsAsync(Guid userId)
    {
        var chats = await _db.Chats
            .Include(c => c.Match).ThenInclude(m => m.User1)
            .Include(c => c.Match).ThenInclude(m => m.User2)
            .Include(c => c.Messages)
            .Where(c => !c.IsDeleted &&
                (c.Match.User1Id == userId || c.Match.User2Id == userId) &&
                c.Match.IsActive)
            .OrderByDescending(c => c.Messages.Where(m => !m.IsDeleted).Max(m => (DateTime?)m.CreatedAt) ?? c.CreatedAt)
            .ToListAsync();

        return chats.Select(c =>
        {
            var other = c.Match.User1Id == userId ? c.Match.User2 : c.Match.User1;
            var lastMsg = c.Messages.Where(m => !m.IsDeleted).OrderByDescending(m => m.CreatedAt).FirstOrDefault();
            var unread  = c.Messages.Count(m => !m.IsDeleted && m.SenderId != userId && m.ReadAt == null);
            return new ChatListItemDto
            {
                ChatId    = c.Id.ToString(),
                MatchId   = c.MatchId.ToString(),
                UnreadCount = unread,
                Participant = new ChatParticipantDto
                {
                    Id = other?.Id.ToString(), FullName = other?.FullName,
                    Avatar = other?.Avatar, IsOnline = other?.IsOnline
                },
                LastMessage = lastMsg == null ? null : new ChatMessageDto
                {
                    Id = lastMsg.Id.ToString(), Text = lastMsg.Text,
                    Type = lastMsg.Type, SentAt = lastMsg.CreatedAt, ReadAt = lastMsg.ReadAt
                }
            };
        }).ToList();
    }

    public async Task<List<ChatMessageDto>> GetMessagesAsync(Guid userId, Guid chatId, int page)
    {
        var chat = await _db.Chats.Include(c => c.Match)
            .FirstOrDefaultAsync(c => c.Id == chatId && !c.IsDeleted &&
                (c.Match.User1Id == userId || c.Match.User2Id == userId))
            ?? throw new InvalidOperationException("Chat not found.");

        const int pageSize = 50;
        var messages = await _db.Messages
            .Where(m => m.ChatId == chatId && !m.IsDeleted)
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync();

        return messages.Select(MapMessage).ToList();
    }

    public async Task<SendMessageResponse> SendMessageAsync(Guid senderId, Guid chatId, SendMessageRequest req)
    {
        var chat = await _db.Chats.Include(c => c.Match)
            .FirstOrDefaultAsync(c => c.Id == chatId && !c.IsDeleted &&
                (c.Match.User1Id == senderId || c.Match.User2Id == senderId))
            ?? throw new InvalidOperationException("Chat not found.");

        var sender = await _db.Users.FindAsync(senderId)
            ?? throw new InvalidOperationException("User not found.");

        int coinsDeducted = 0;

        if (sender.Gender?.ToLower() == "male")
        {
            var cost = sender.IsPremium ? MalePremiumCostPerMessage : MaleCostPerMessage;
            if (sender.CoinBalance < cost)
                throw new InvalidOperationException($"Insufficient coins. Need {cost} coins to send a message.");
            await _wallet.DeductCoinsAsync(senderId, cost, "Message sent", "message");
            coinsDeducted = cost;
        }
        else
        {
            var sentCount = await _db.Messages
                .CountAsync(m => m.ChatId == chatId && m.SenderId == senderId && !m.IsDeleted);
            if (sentCount >= FemaleFreeMessages)
            {
                if (sender.CoinBalance < FemaleMessageCost)
                    throw new InvalidOperationException($"Insufficient coins. Need {FemaleMessageCost} coins.");
                await _wallet.DeductCoinsAsync(senderId, FemaleMessageCost, "Message sent", "message");
                coinsDeducted = FemaleMessageCost;
            }
        }

        var message = new Message
        {
            ChatId = chatId, SenderId = senderId,
            Text = req.Text, Type = req.Type, ImageUrl = req.ImageUrl,
            CoinsDeducted = coinsDeducted
        };
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        var updatedSender = await _db.Users.FindAsync(senderId);
        var remaining = await GetRemainingQuota(senderId, chatId, sender.Gender ?? "");

        return new SendMessageResponse
        {
            Id = message.Id.ToString(),
            CoinsDeducted = coinsDeducted,
            NewBalance = updatedSender?.CoinBalance,
            Remaining = remaining
        };
    }

    public async Task MarkReadAsync(Guid userId, Guid chatId)
    {
        var messages = await _db.Messages
            .Where(m => m.ChatId == chatId && m.SenderId != userId && m.ReadAt == null && !m.IsDeleted)
            .ToListAsync();
        foreach (var msg in messages) msg.ReadAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(Guid userId, Guid chatId, Guid messageId)
    {
        var message = await _db.Messages.FirstOrDefaultAsync(m =>
            m.Id == messageId && m.ChatId == chatId && m.SenderId == userId && !m.IsDeleted)
            ?? throw new InvalidOperationException("Message not found.");
        message.IsDeleted = true;
        message.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<ChatQuotaDto> GetQuotaAsync(Guid userId, Guid chatId)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        int freeRemaining = 0;
        int remaining;
        int cost;

        if (user.Gender?.ToLower() == "male")
        {
            cost = user.IsPremium ? MalePremiumCostPerMessage : MaleCostPerMessage;
            remaining = user.CoinBalance / cost;
        }
        else
        {
            var sentCount = await _db.Messages.CountAsync(m =>
                m.ChatId == chatId && m.SenderId == userId && !m.IsDeleted);
            cost = FemaleMessageCost;
            freeRemaining = Math.Max(0, FemaleFreeMessages - sentCount);
            remaining = freeRemaining > 0 ? freeRemaining : (user.CoinBalance / cost);
        }

        return new ChatQuotaDto
        {
            FreeRemaining = freeRemaining,
            Remaining = remaining,
            IsPremium = user.IsPremium,
            CostPerMessage = cost
        };
    }

    private static ChatMessageDto MapMessage(Message m) => new()
    {
        Id = m.Id.ToString(), SenderId = m.SenderId.ToString(),
        Text = m.Text, Type = m.Type, ImageUrl = m.ImageUrl,
        GiftName = m.GiftName, GiftCost = m.GiftCost,
        CoinAmount = m.CoinAmount, SentAt = m.CreatedAt,
        ReadAt = m.ReadAt, CoinsDeducted = m.CoinsDeducted
    };

    private async Task<int> GetRemainingQuota(Guid userId, Guid chatId, string gender)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return 0;
        if (gender.ToLower() == "male")
        {
            var cost = user.IsPremium ? MalePremiumCostPerMessage : MaleCostPerMessage;
            return user.CoinBalance / cost;
        }
        var sentCount = await _db.Messages.CountAsync(m => m.ChatId == chatId && m.SenderId == userId && !m.IsDeleted);
        return sentCount < FemaleFreeMessages ? FemaleFreeMessages - sentCount : user.CoinBalance / FemaleMessageCost;
    }
}
