using Mingley.Application.DTOs.Discover;

namespace Mingley.Application.Interfaces;

public interface IDiscoverService
{
    Task<(List<DiscoverUserDto> Users, PaginationDto Pagination)> GetFeedAsync(Guid userId, int page, int limit);
    Task<SwipeResponse> SwipeAsync(Guid swiperId, SwipeRequest request);
    Task<List<MatchListItemDto>> GetMatchesAsync(Guid userId, int page, int limit);
    Task UnmatchAsync(Guid userId, Guid matchId);
}
