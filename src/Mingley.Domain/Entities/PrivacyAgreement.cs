using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

/// <summary>Tracks when user accepted privacy popup after match</summary>
public class PrivacyAgreement : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid MatchId { get; set; }
    public bool Accepted { get; set; } = true;
    public User? User { get; set; }
}
