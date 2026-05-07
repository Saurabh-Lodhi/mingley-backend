using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }

    /// <summary>match | message | like | system</summary>
    public string? Type { get; set; }

    public bool IsRead { get; set; } = false;
    public string? ReferenceId { get; set; }
    public User? User { get; set; }
}
