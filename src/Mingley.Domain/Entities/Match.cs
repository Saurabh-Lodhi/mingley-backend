using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Match : BaseEntity
{
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    public bool IsActive { get; set; } = true;

    public User? User1 { get; set; }
    public User? User2 { get; set; }
    public Chat? Chat { get; set; }
}
