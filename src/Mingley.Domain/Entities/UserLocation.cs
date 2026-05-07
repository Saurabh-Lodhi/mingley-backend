using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

public class UserLocation : BaseEntity
{
    public Guid UserId { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public User? User { get; set; }
}
