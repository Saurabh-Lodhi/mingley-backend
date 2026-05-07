using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class UserImage : BaseEntity
{
    public Guid UserId { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
    public User? User { get; set; }
}
