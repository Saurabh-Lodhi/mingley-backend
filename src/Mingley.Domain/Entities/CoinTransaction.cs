using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class CoinTransaction : BaseEntity
{
    public Guid UserId { get; set; }
    public int Coins { get; set; }

    /// <summary>credit | debit</summary>
    public string Direction { get; set; } = "credit";

    public string? Description { get; set; }

    /// <summary>message | gift | deposit | withdrawal | subscription</summary>
    public string? TransactionType { get; set; }

    public string? ReferenceId { get; set; }
    public User? User { get; set; }
}
