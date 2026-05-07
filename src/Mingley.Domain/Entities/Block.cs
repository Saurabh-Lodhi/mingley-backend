using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Block : BaseEntity
{
    public Guid BlockerId { get; set; }
    public Guid BlockedUserId { get; set; }
    public User? Blocker { get; set; }
    public User? BlockedUser { get; set; }
}
