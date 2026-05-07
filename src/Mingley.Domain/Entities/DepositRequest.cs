using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

/// <summary>Matches DepositModal — UTR ID submission for coin top-up.</summary>
public class DepositRequest : BaseEntity
{
    public Guid UserId { get; set; }
    public string? UtrId { get; set; }
    public string? ScreenshotUrl { get; set; }
    public int? RequestedCoins { get; set; }

    /// <summary>pending | approved | rejected</summary>
    public string Status { get; set; } = "pending";

    public string? AdminNote { get; set; }
    public User? User { get; set; }
}
