using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Report : BaseEntity
{
    public Guid ReporterId { get; set; }
    public Guid ReportedUserId { get; set; }
    public string? Reason { get; set; }

    /// <summary>pending | reviewed | dismissed</summary>
    public string Status { get; set; } = "pending";

    public User? Reporter { get; set; }
    public User? ReportedUser { get; set; }
}
