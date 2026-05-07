using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Interest : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
}
