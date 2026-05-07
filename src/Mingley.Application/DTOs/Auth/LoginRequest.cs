namespace Mingley.Application.DTOs.Auth;

/// <summary>Matches LoginScreen.js login payload.</summary>
public class LoginRequest
{
    public string Identifier { get; set; } = string.Empty; // email or phone
    public string Password { get; set; } = string.Empty;
    public string? TwoFactorCode { get; set; }
    public string? DeviceToken { get; set; }
    public string? DevicePlatform { get; set; }
}
