using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

/// <summary>Discover filter preferences — matches FilterSheet.js fields.</summary>
public class UserPreference : BaseEntity
{
    public Guid UserId { get; set; }

    /// <summary>girls | boys | both</summary>
    public string? InterestedIn { get; set; } = "both";

    public int? MinAge { get; set; } = 18;
    public int? MaxAge { get; set; } = 40;
    public int? MaxDistance { get; set; } = 50;

    /// <summary>casual | serious | both</summary>
    public string? RelationshipType { get; set; } = "both";

    public bool? NearbyOnly { get; set; } = false;
    public bool? OnlineOnly { get; set; } = false;
    public bool? VerifiedOnly { get; set; } = false;
    public string? Location { get; set; }

    public User? User { get; set; }
}
