using Mingley.Application.DTOs.Auth;

namespace Mingley.Application.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> VerifyOtpAsync(VerifyOtpRequest request);
    Task ResendOtpAsync(ResendOtpRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(Guid userId);
    Task ForgotPasswordAsync(ForgotPasswordRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
}
