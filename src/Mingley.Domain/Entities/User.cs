using Mingley.Domain.Common;

namespace Mingley.Domain.Entities;

/// <summary>Core user entity matching all frontend fields.</summary>
public class User : BaseEntity
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PasswordHash { get; set; }

    /// <summary>male | female | other</summary>
    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? Bio { get; set; }
    public string? Avatar { get; set; }

    /// <summary>user | admin</summary>
    public string Role { get; set; } = "user";

    public bool IsVerified { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public int CoinBalance { get; set; } = 0;
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    public DateTime? LastActiveAt { get; set; }
    public bool IsOnline { get; set; } = false;

    // OTP fields (nullable — not always needed)
    public string? OtpCode { get; set; }
    public DateTime? OtpExpiry { get; set; }
    public string? OtpPurpose { get; set; }

    // Navigation
    public UserLocation? Location { get; set; }
    public UserPreference? Preference { get; set; }
    public ICollection<UserImage> Images { get; set; } = new List<UserImage>();
    public ICollection<UserInterest> Interests { get; set; } = new List<UserInterest>();
}
