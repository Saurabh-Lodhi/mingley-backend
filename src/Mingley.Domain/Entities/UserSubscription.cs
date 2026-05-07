using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class UserSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool AutoRenew { get; set; } = true;
    public string? CancelReason { get; set; }

    public User? User { get; set; }
    public SubscriptionPlan? Plan { get; set; }
}
