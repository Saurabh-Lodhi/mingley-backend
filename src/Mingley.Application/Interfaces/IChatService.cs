using Mingley.Application.DTOs.Chat;

namespace Mingley.Application.Interfaces;

public interface IChatService
{
    Task<List<ChatListItemDto>> GetChatsAsync(Guid userId);
    Task<List<ChatMessageDto>> GetMessagesAsync(Guid userId, Guid chatId, int page);
    Task<SendMessageResponse> SendMessageAsync(Guid senderId, Guid chatId, SendMessageRequest request);
    Task MarkReadAsync(Guid userId, Guid chatId);
    Task DeleteMessageAsync(Guid userId, Guid chatId, Guid messageId);
    Task<ChatQuotaDto> GetQuotaAsync(Guid userId, Guid chatId);
}
