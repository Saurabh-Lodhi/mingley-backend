namespace Mingley.Application.DTOs.Users;

/// <summary>Full profile — used by GET /users/me and GET /users/{id}</summary>
public class UserProfileDto
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public int? Age { get; set; }
    public string? Bio { get; set; }
    public string? Avatar { get; set; }
    public bool? IsVerified { get; set; }
    public bool? IsPremium { get; set; }
    public bool? IsOnline { get; set; }
    public int? CoinBalance { get; set; }
    public string? Role { get; set; }
    public bool? TwoFactorEnabled { get; set; }
    public DateTime? LastActiveAt { get; set; }
    public LocationDto? Location { get; set; }
    public PreferenceDto? Preference { get; set; }
    public List<string>? Interests { get; set; }
    public List<ImageDto>? Images { get; set; }
}

public class LocationDto
{
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

public class PreferenceDto
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

public class ImageDto
{
    public string? Id { get; set; }
    public string? Url { get; set; }
    public int? SortOrder { get; set; }
}
