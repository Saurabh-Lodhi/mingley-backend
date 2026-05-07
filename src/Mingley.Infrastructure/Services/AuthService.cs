using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mingley.Application.DTOs.Auth;
using Mingley.Application.Interfaces;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;

namespace Mingley.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly MingleyDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public AuthService(MingleyDbContext db, ITokenService tokenService, IConfiguration config)
    {
        _db = db;
        _tokenService = tokenService;
        _config = config;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest req)
    {
        // Validate no duplicate email/phone
        var exists = await _db.Users.AnyAsync(u =>
            (req.Email != null && u.Email == req.Email) ||
            (req.Phone != null && u.Phone == req.Phone));

        if (exists) throw new InvalidOperationException("Email or phone already registered.");

        if (req.Password != req.ConfirmPassword)
            throw new InvalidOperationException("Passwords do not match.");

        var otp = GenerateOtp();
        var user = new User
        {
            FullName = req.FullName,
            Email = req.Email?.ToLower().Trim(),
            Phone = req.Phone?.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Gender = req.Gender,
            DateOfBirth = req.DateOfBirth?.ToUniversalTime(),
            OtpCode = otp,
            OtpExpiry = DateTime.UtcNow.AddMinutes(10),
            OtpPurpose = "registration",
            Role = "user"
        };

        _db.Users.Add(user);
        // Create default preference
        _db.UserPreferences.Add(new UserPreference { UserId = user.Id });
        await _db.SaveChangesAsync();

        // TODO: Send OTP via email/SMS in production
        // For dev: return OTP in response
        var isDev = _config["App:Environment"] == "Development";
        return new RegisterResponse
        {
            UserId = user.Id.ToString(),
            DevOtp = isDev ? otp : null
        };
    }

    public async Task<AuthResponse> VerifyOtpAsync(VerifyOtpRequest req)
    {
        if (!Guid.TryParse(req.UserId, out var userId))
            throw new InvalidOperationException("Invalid user ID.");

        var user = await _db.Users
            .Include(u => u.Location)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted)
            ?? throw new InvalidOperationException("User not found.");

        if (user.OtpCode != req.Otp)
            throw new InvalidOperationException("Invalid OTP.");

        if (user.OtpExpiry < DateTime.UtcNow)
            throw new InvalidOperationException("OTP expired. Please request a new one.");

        if (user.OtpPurpose != req.Purpose)
            throw new InvalidOperationException("OTP purpose mismatch.");

        // Verify and clear OTP
        user.IsVerified = true;
        user.OtpCode = null;
        user.OtpExpiry = null;
        user.OtpPurpose = null;
        user.LastActiveAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return BuildAuthResponse(user);
    }

    public async Task ResendOtpAsync(ResendOtpRequest req)
    {
        if (!Guid.TryParse(req.UserId, out var userId))
            throw new InvalidOperationException("Invalid user ID.");

        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        user.OtpCode = GenerateOtp();
        user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);
        user.OtpPurpose = req.Purpose;

        await _db.SaveChangesAsync();
        // TODO: Send OTP via email/SMS
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest req)
    {
        var identifier = req.Identifier.ToLower().Trim();
        var user = await _db.Users
            .Include(u => u.Location)
            .FirstOrDefaultAsync(u =>
                !u.IsDeleted && u.IsActive &&
                (u.Email == identifier || u.Phone == identifier))
            ?? throw new InvalidOperationException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid email or password.");

        // 2FA check
        if (user.TwoFactorEnabled)
        {
            if (string.IsNullOrEmpty(req.TwoFactorCode))
                return new AuthResponse { Requires2FA = true, UserId = user.Id.ToString() };

            var totp = new OtpNet.Totp(OtpNet.Base32Encoding.ToBytes(user.TwoFactorSecret!));
            if (!totp.VerifyTotp(req.TwoFactorCode, out _))
                throw new InvalidOperationException("Invalid 2FA code.");
        }

        user.LastActiveAt = DateTime.UtcNow;
        user.IsOnline = true;
        await _db.SaveChangesAsync();

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var userId = _tokenService.ValidateRefreshToken(refreshToken)
            ?? throw new InvalidOperationException("Invalid or expired refresh token.");

        var user = await _db.Users.Include(u => u.Location)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted && u.IsActive)
            ?? throw new InvalidOperationException("User not found.");

        _tokenService.RevokeRefreshToken(refreshToken);
        return BuildAuthResponse(user);
    }

    public async Task LogoutAsync(Guid userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user != null)
        {
            user.IsOnline = false;
            user.LastActiveAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest req)
    {
        var identifier = req.Identifier.ToLower().Trim();
        var user = await _db.Users.FirstOrDefaultAsync(u =>
            !u.IsDeleted && (u.Email == identifier || u.Phone == identifier));

        if (user == null) return; // Silent fail for security

        user.OtpCode = GenerateOtp();
        user.OtpExpiry = DateTime.UtcNow.AddMinutes(15);
        user.OtpPurpose = "forgot_password";
        await _db.SaveChangesAsync();
        // TODO: Send OTP
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest req)
    {
        if (!Guid.TryParse(req.UserId, out var userId))
            throw new InvalidOperationException("Invalid user ID.");

        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        if (user.OtpCode != req.Otp || user.OtpPurpose != "forgot_password")
            throw new InvalidOperationException("Invalid OTP.");

        if (user.OtpExpiry < DateTime.UtcNow)
            throw new InvalidOperationException("OTP expired.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
        user.OtpCode = null;
        user.OtpExpiry = null;
        user.OtpPurpose = null;
        await _db.SaveChangesAsync();
    }

    // ── Helpers ────────────────────────────────────────────────
    private AuthResponse BuildAuthResponse(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        _tokenService.StoreRefreshToken(user.Id, refreshToken);

        return new AuthResponse
        {
            UserId = user.Id.ToString(),
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            ExpiresIn = 3600,
            User = new UserDto
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Gender = user.Gender,
                Avatar = user.Avatar,
                IsPremium = user.IsPremium,
                IsVerified = user.IsVerified,
                CoinBalance = user.CoinBalance,
                Role = user.Role,
                TwoFactorEnabled = user.TwoFactorEnabled,
                IsOnline = user.IsOnline,
                LastActiveAt = user.LastActiveAt,
                ProfileComplete = !string.IsNullOrEmpty(user.FullName) && !string.IsNullOrEmpty(user.Gender)
            }
        };
    }

    private static string GenerateOtp() =>
        Random.Shared.Next(100000, 999999).ToString();
}
