namespace Mingley.Application.DTOs.Auth;

/// <summary>JWT token response — matches frontend token storage in AsyncStorage.</summary>
public class AuthResponse
{
    public string? UserId { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } = 3600;
    public UserDto? User { get; set; }
    public bool? Requires2FA { get; set; }
}

public class RegisterResponse
{
    public string? UserId { get; set; }
    public string? DevOtp { get; set; } // Only in development
}

public class UserDto
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? Avatar { get; set; }
    public bool? IsPremium { get; set; }
    public bool? IsVerified { get; set; }
    public int? CoinBalance { get; set; }
    public string? Role { get; set; }
    public bool? ProfileComplete { get; set; }
    public bool? TwoFactorEnabled { get; set; }
    public bool? IsOnline { get; set; }
    public DateTime? LastActiveAt { get; set; }
}
