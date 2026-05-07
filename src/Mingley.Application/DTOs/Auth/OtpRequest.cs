namespace Mingley.Application.DTOs.Auth;

public class VerifyOtpRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
    public string Purpose { get; set; } = "registration"; // registration | forgot_password
}

public class ResendOtpRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Purpose { get; set; } = "registration";
}

public class ForgotPasswordRequest
{
    public string Identifier { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
