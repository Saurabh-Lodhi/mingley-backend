namespace Mingley.Application.DTOs.Users;

/// <summary>Profile update — all fields nullable so partial updates work.</summary>
public class UpdateProfileRequest
{
    public string? FullName { get; set; }
    public string? Bio { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Avatar { get; set; }
}

public class UpdateInterestsRequest
{
    public List<string> Interests { get; set; } = new();
}

public class UpdateLocationRequest
{
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

public class UpdatePreferencesRequest
{
    public string? InterestedIn { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? MaxDistance { get; set; }
    public string? RelationshipType { get; set; }
    public bool? NearbyOnly { get; set; }
    public bool? OnlineOnly { get; set; }
    public bool? VerifiedOnly { get; set; }
    public string? Location { get; set; }
}
