using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

/// <summary>Matches SubscriptionPlansScreen.js — plan.name, plan.price, plan.durationDays, plan.features, plan.isPopular</summary>
public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }

    /// <summary>JSON array of feature strings e.g. ["Unlimited likes","No ads"]</summary>
    public string? Features { get; set; }

    public bool IsPopular { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
