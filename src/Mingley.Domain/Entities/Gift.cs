using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class Gift : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int CoinCost { get; set; }
    public bool IsActive { get; set; } = true;
}
