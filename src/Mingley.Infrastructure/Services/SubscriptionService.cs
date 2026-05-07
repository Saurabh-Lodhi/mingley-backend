using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Subscription;
using Mingley.Application.Interfaces;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;

namespace Mingley.Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly MingleyDbContext _db;
    private readonly IMapper _mapper;
    private readonly IWalletService _wallet;

    public SubscriptionService(MingleyDbContext db, IMapper mapper, IWalletService wallet)
    {
        _db = db; _mapper = mapper; _wallet = wallet;
    }

    public async Task<List<SubscriptionPlanDto>> GetPlansAsync()
    {
        var plans = await _db.SubscriptionPlans
            .Where(p => p.IsActive && !p.IsDeleted)
            .OrderBy(p => p.Price)
            .ToListAsync();
        return _mapper.Map<List<SubscriptionPlanDto>>(plans);
    }

    public async Task<UserSubscriptionDto?> GetStatusAsync(Guid userId)
    {
        var sub = await _db.UserSubscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive && s.EndDate > DateTime.UtcNow);

        if (sub == null) return null;
        return new UserSubscriptionDto
        {
            Id = sub.Id.ToString(),
            PlanName = sub.Plan?.Name,
            StartDate = sub.StartDate,
            EndDate = sub.EndDate,
            IsActive = sub.IsActive,
            AutoRenew = sub.AutoRenew
        };
    }

    public async Task<SubscribeResponse> SubscribeAsync(Guid userId, SubscribeRequest req)
    {
        if (!Guid.TryParse(req.PlanId, out var planId))
            throw new InvalidOperationException("Invalid plan ID.");

        var plan = await _db.SubscriptionPlans.FindAsync(planId)
            ?? throw new InvalidOperationException("Plan not found.");

        // Deactivate old subscriptions
        var old = await _db.UserSubscriptions
            .Where(s => s.UserId == userId && s.IsActive)
            .ToListAsync();
        old.ForEach(s => s.IsActive = false);

        var endDate = DateTime.UtcNow.AddDays(plan.DurationDays);
        var subscription = new UserSubscription
        {
            UserId = userId,
            PlanId = planId,
            StartDate = DateTime.UtcNow,
            EndDate = endDate,
            IsActive = true,
            AutoRenew = req.AutoRenew
        };

        _db.UserSubscriptions.Add(subscription);

        // Mark user as premium
        var user = await _db.Users.FindAsync(userId);
        if (user != null) { user.IsPremium = true; user.UpdatedAt = DateTime.UtcNow; }

        await _db.SaveChangesAsync();

        return new SubscribeResponse
        {
            SubscriptionId = subscription.Id.ToString(),
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            IsActive = true
        };
    }

    public async Task CancelAsync(Guid userId, Guid subscriptionId, string? reason)
    {
        var sub = await _db.UserSubscriptions
            .FirstOrDefaultAsync(s => s.Id == subscriptionId && s.UserId == userId && s.IsActive)
            ?? throw new InvalidOperationException("Subscription not found.");

        sub.IsActive = false;
        sub.AutoRenew = false;
        sub.CancelReason = reason;
        sub.UpdatedAt = DateTime.UtcNow;

        var user = await _db.Users.FindAsync(userId);
        if (user != null) { user.IsPremium = false; user.UpdatedAt = DateTime.UtcNow; }

        await _db.SaveChangesAsync();
    }
}
