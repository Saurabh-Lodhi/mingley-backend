namespace Mingley.Application.DTOs.Chat;

public class ChatListItemDto
{
    public string? ChatId { get; set; }
    public string? MatchId { get; set; }
    public ChatParticipantDto? Participant { get; set; }
    public ChatMessageDto? LastMessage { get; set; }
    public int? UnreadCount { get; set; }
}

public class ChatParticipantDto
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? Avatar { get; set; }
    public bool? IsOnline { get; set; }
}

public class ChatMessageDto
{
    public string? Id { get; set; }
    public string? SenderId { get; set; }
    public string? Text { get; set; }
    public string? Type { get; set; }
    public string? ImageUrl { get; set; }
    public string? GiftName { get; set; }
    public int? GiftCost { get; set; }
    public int? CoinAmount { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public int? CoinsDeducted { get; set; }
}

public class SendMessageRequest
{
    public string? Text { get; set; }
    public string Type { get; set; } = "text";
    public string? ImageUrl { get; set; }
}

public class SendMessageResponse
{
    public string? Id { get; set; }
    public int? CoinsDeducted { get; set; }
    public int? NewBalance { get; set; }
    public int? Remaining { get; set; }
}

public class ChatQuotaDto
{
    public int? FreeRemaining { get; set; }
    public int? Remaining { get; set; }
    public bool? IsPremium { get; set; }
    public int? CostPerMessage { get; set; }
}
