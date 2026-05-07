using Mingley.Application.DTOs.Users;

namespace Mingley.Application.Interfaces;

public interface IUserService
{
    Task<UserProfileDto?> GetMeAsync(Guid userId);
    Task<UserProfileDto?> GetUserAsync(Guid userId, Guid requesterId);
    Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
    Task UpdateInterestsAsync(Guid userId, List<string> interests);
    Task UpdatePreferencesAsync(Guid userId, UpdatePreferencesRequest request);
    Task UpdateLocationAsync(Guid userId, UpdateLocationRequest request);
    Task AddImageAsync(Guid userId, string url);
    Task DeleteImageAsync(Guid userId, Guid imageId);
    Task BlockUserAsync(Guid blockerId, Guid targetId);
    Task UnblockUserAsync(Guid blockerId, Guid targetId);
    Task<List<UserProfileDto>> GetBlockedUsersAsync(Guid userId);
}
