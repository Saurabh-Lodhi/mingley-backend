using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mingley.Application.Interfaces;
using Mingley.Domain.Entities;

namespace Mingley.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    // In-memory store for refresh tokens (use Redis or DB in production)
    private static readonly Dictionary<string, Guid> _refreshTokens = new();

    public TokenService(IConfiguration config) => _config = config;

    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Role, user.Role),
            new("gender", user.Gender ?? ""),
            new("isPremium", user.IsPremium.ToString().ToLower()),
            new("isVerified", user.IsVerified.ToString().ToLower()),
            new("fullName", user.FullName ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public Guid? ValidateRefreshToken(string refreshToken)
    {
        return _refreshTokens.TryGetValue(refreshToken, out var userId) ? userId : null;
    }

    public void StoreRefreshToken(Guid userId, string refreshToken)
    {
        _refreshTokens[refreshToken] = userId;
    }

    public void RevokeRefreshToken(string refreshToken)
    {
        _refreshTokens.Remove(refreshToken);
    }
}
