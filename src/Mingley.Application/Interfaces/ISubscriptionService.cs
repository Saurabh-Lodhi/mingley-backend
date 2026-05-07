using Mingley.Application.DTOs.Subscription;

namespace Mingley.Application.Interfaces;

public interface ISubscriptionService
{
    Task<List<SubscriptionPlanDto>> GetPlansAsync();
    Task<UserSubscriptionDto?> GetStatusAsync(Guid userId);
    Task<SubscribeResponse> SubscribeAsync(Guid userId, SubscribeRequest request);
    Task CancelAsync(Guid userId, Guid subscriptionId, string? reason);
}
