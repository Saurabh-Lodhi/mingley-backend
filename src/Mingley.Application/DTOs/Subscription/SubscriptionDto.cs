namespace Mingley.Application.DTOs.Subscription;

/// <summary>Matches SubscriptionPlansScreen.js plan object exactly.</summary>
public class SubscriptionPlanDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? DurationDays { get; set; }

    /// <summary>JSON string of feature array — frontend parses JSON.parse(plan.features)</summary>
    public string? Features { get; set; }

    public bool? IsPopular { get; set; }
}

public class SubscribeRequest
{
    public string PlanId { get; set; } = string.Empty;
    public bool AutoRenew { get; set; } = true;
}

public class SubscribeResponse
{
    public string? SubscriptionId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
}

public class UserSubscriptionDto
{
    public string? Id { get; set; }
    public string? PlanName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
    public bool? AutoRenew { get; set; }
}
