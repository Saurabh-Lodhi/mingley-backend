using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mingley.Application.DTOs.Auth;
using Mingley.Application.DTOs.Common;
using Mingley.Application.Interfaces;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>Register a new user — matches EmailInputScreen.js</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<RegisterResponse>), 201)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        try
        {
            var result = await _auth.RegisterAsync(req);
            return StatusCode(201, ApiResponse<RegisterResponse>.Created(result, "Registration successful. Please verify your OTP."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Verify OTP — matches OTPVerificationScreen.js</summary>
    [HttpPost("verify-otp")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), 200)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest req)
    {
        try
        {
            var result = await _auth.VerifyOtpAsync(req);
            return Ok(ApiResponse<AuthResponse>.Ok(result, "OTP verified successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Resend OTP — matches OTPVerificationScreen.js resend button</summary>
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequest req)
    {
        await _auth.ResendOtpAsync(req);
        return Ok(ApiResponse.Ok("OTP sent successfully."));
    }

    /// <summary>Login — matches LoginScreen.js</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), 200)]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        try
        {
            var result = await _auth.LoginAsync(req);
            return Ok(ApiResponse<AuthResponse>.Ok(result, "Login successful."));
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(ApiResponse<object>.Fail(ex.Message, 401));
        }
    }

    /// <summary>Refresh access token using refresh token</summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest req)
    {
        try
        {
            var result = await _auth.RefreshTokenAsync(req.RefreshToken);
            return Ok(ApiResponse<AuthResponse>.Ok(result));
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(ApiResponse<object>.Fail(ex.Message, 401));
        }
    }

    /// <summary>Logout — clears online status</summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _auth.LogoutAsync(userId);
        return Ok(ApiResponse.Ok("Logged out successfully."));
    }

    /// <summary>Forgot password — sends OTP to email/phone</summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
    {
        await _auth.ForgotPasswordAsync(req);
        return Ok(ApiResponse.Ok("If the account exists, an OTP has been sent."));
    }

    /// <summary>Reset password using OTP</summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
    {
        try
        {
            await _auth.ResetPasswordAsync(req);
            return Ok(ApiResponse.Ok("Password reset successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
