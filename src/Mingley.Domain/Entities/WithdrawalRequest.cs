using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

/// <summary>Matches CashoutModal — female users withdrawing coins.</summary>
public class WithdrawalRequest : BaseEntity
{
    public Guid UserId { get; set; }
    public int Coins { get; set; }
    public string? BankOrUpi { get; set; }

    /// <summary>pending | approved | rejected</summary>
    public string Status { get; set; } = "pending";

    public string? AdminNote { get; set; }
    public User? User { get; set; }
}
