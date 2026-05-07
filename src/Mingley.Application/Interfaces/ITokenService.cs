using Mingley.Domain.Entities;

namespace Mingley.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Guid? ValidateRefreshToken(string refreshToken);
    void StoreRefreshToken(Guid userId, string refreshToken);
    void RevokeRefreshToken(string refreshToken);
}
