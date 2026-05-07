using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Chat : BaseEntity
{
    public Guid MatchId { get; set; }
    public Match? Match { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
