namespace Mingley.Application.DTOs.Auth;

/// <summary>Matches EmailInputScreen.js register payload.</summary>
public class RegisterRequest
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? ConfirmPassword { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
