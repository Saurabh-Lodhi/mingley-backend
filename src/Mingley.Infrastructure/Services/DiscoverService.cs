using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Discover;
using Mingley.Application.Interfaces;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;

namespace Mingley.Infrastructure.Services;

public class DiscoverService : IDiscoverService
{
    private readonly MingleyDbContext _db;
    public DiscoverService(MingleyDbContext db) => _db = db;

    public async Task<(List<DiscoverUserDto> Users, PaginationDto Pagination)> GetFeedAsync(
        Guid userId, int page, int limit)
    {
        var me = await _db.Users
            .Include(u => u.Preference)
            .Include(u => u.Location)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (me == null) return (new(), new PaginationDto());

        var pref = me.Preference;

        // GENDER DEFAULT: male sees female, female sees male
        //string targetGender;
        //if (pref?.InterestedIn != null && pref.InterestedIn != "both")
        //    targetGender = pref.InterestedIn == "girls" || pref.InterestedIn == "female" ? "female" : "male";
        //else
        //    targetGender = me.Gender?.ToLower() == "male" ? "female" : "male";
        string targetGender;
        var interestedIn = pref?.InterestedIn?.ToLower();
        if (interestedIn == "girls" || interestedIn == "female")
            targetGender = "female";
        else if (interestedIn == "boys" || interestedIn == "male")
            targetGender = "male";
        else
            // Default: male sees female, female sees male
            targetGender = me.Gender?.ToLower() == "male" ? "female" : "male";

        var blockedIds = await _db.Blocks
            .Where(b => b.BlockerId == userId || b.BlockedUserId == userId)
            .Select(b => b.BlockerId == userId ? b.BlockedUserId : b.BlockerId)
            .ToListAsync();

        var swipedIds = await _db.Swipes
            .Where(s => s.SwiperId == userId)
            .Select(s => s.TargetId)
            .ToListAsync();

        var excludeIds = blockedIds.Union(swipedIds).Append(userId).ToHashSet();

        var query = _db.Users
            .Include(u => u.Location)
            .Include(u => u.Images)
            .Include(u => u.Interests).ThenInclude(i => i.Interest)
            .Where(u => !u.IsDeleted && u.IsActive && !excludeIds.Contains(u.Id)
                && u.Gender!.ToLower() == targetGender);

        // Age filter
        if (pref?.MinAge.HasValue == true)
        {
            var maxDob = DateTime.UtcNow.AddYears(-pref.MinAge.Value);
            query = query.Where(u => u.DateOfBirth <= maxDob);
        }
        if (pref?.MaxAge.HasValue == true)
        {
            var minDob = DateTime.UtcNow.AddYears(-pref.MaxAge.Value - 1);
            query = query.Where(u => u.DateOfBirth >= minDob);
        }
        if (me.IsPremium && pref?.VerifiedOnly == true)
            query = query.Where(u => u.IsVerified);
        if (pref?.OnlineOnly == true)
            query = query.Where(u => u.IsOnline);

        var total = await query.CountAsync();
        var users = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

        var rng = new Random();
        var dtos = users.Select(u => new DiscoverUserDto
        {
            Id        = u.Id.ToString(),
            FullName  = u.FullName,
            Age       = u.DateOfBirth.HasValue ? (int?)((DateTime.UtcNow - u.DateOfBirth.Value).Days / 365) : null,
            Bio       = u.Bio,
            Gender    = u.Gender,
            Avatar    = u.Avatar ?? $"https://randomuser.me/api/portraits/{(u.Gender=="female"?"women":"men")}/{rng.Next(1,70)}.jpg",
            IsVerified = u.IsVerified,
            IsPremium = u.IsPremium,
            IsOnline  = u.IsOnline,
            City      = u.Location?.City,
            Distance  = CalculateDistance(me.Location, u.Location),
            MatchScore = CalculateMatchScore(me, u),
            Interests = u.Interests.Select(i => i.Interest?.Name ?? "").Where(n => n != "").ToList(),
            Images    = u.Images.Where(i => !i.IsDeleted).Select(i => (string?)i.Url).ToList()
        }).ToList();

        return (dtos, new PaginationDto { Page = page, Limit = limit, Total = total, HasNext = (page * limit) < total });
    }

    public async Task<SwipeResponse> SwipeAsync(Guid swiperId, SwipeRequest req)
    {
        if (!Guid.TryParse(req.TargetId, out var targetId))
            throw new InvalidOperationException("Invalid target ID.");

        var existing = await _db.Swipes.FirstOrDefaultAsync(s => s.SwiperId == swiperId && s.TargetId == targetId);
        if (existing == null)
        {
            _db.Swipes.Add(new Swipe { SwiperId = swiperId, TargetId = targetId, Action = req.Action });
            await _db.SaveChangesAsync();
        }

        if (req.Action is "like" or "superlike")
        {
            var mutual = await _db.Swipes.FirstOrDefaultAsync(s =>
                s.SwiperId == targetId && s.TargetId == swiperId && (s.Action == "like" || s.Action == "superlike"));

            if (mutual != null)
            {
                var matchExists = await _db.Matches.AnyAsync(m =>
                    (m.User1Id == swiperId && m.User2Id == targetId) ||
                    (m.User1Id == targetId && m.User2Id == swiperId));

                if (!matchExists)
                {
                    var match = new Match { User1Id = swiperId, User2Id = targetId };
                    _db.Matches.Add(match);
                    await _db.SaveChangesAsync();

                    var chat = new Chat { MatchId = match.Id };
                    _db.Chats.Add(chat);

                    // Add notifications for both users
                    var swiper = await _db.Users.FindAsync(swiperId);
                    var target = await _db.Users.FindAsync(targetId);

                    _db.Notifications.AddRange(
                        new Notification { UserId = swiperId, Title = "New Match! 🎉", Body = $"You matched with {target?.FullName}!", Type = "match", ReferenceId = match.Id.ToString() },
                        new Notification { UserId = targetId, Title = "New Match! 🎉", Body = $"You matched with {swiper?.FullName}!", Type = "match", ReferenceId = match.Id.ToString() }
                    );

                    await _db.SaveChangesAsync();

                    return new SwipeResponse
                    {
                        IsMatch = true,
                        Match = new MatchDto
                        {
                            MatchId = match.Id.ToString(),
                            ChatId  = chat.Id.ToString(),
                            MatchedAt = match.CreatedAt,
                            User = new MatchedUserDto { Id = target?.Id.ToString(), FullName = target?.FullName, Avatar = target?.Avatar }
                        }
                    };
                }
            }
        }
        return new SwipeResponse { IsMatch = false };
    }

    public async Task<List<MatchListItemDto>> GetMatchesAsync(Guid userId, int page, int limit)
    {
        var matches = await _db.Matches
            .Include(m => m.User1).Include(m => m.User2)
            .Include(m => m.Chat).ThenInclude(c => c!.Messages)
            .Where(m => m.IsActive && !m.IsDeleted && (m.User1Id == userId || m.User2Id == userId))
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * limit).Take(limit)
            .ToListAsync();

        return matches.Select(m =>
        {
            var other = m.User1Id == userId ? m.User2 : m.User1;
            var lastMsg = m.Chat?.Messages.Where(msg => !msg.IsDeleted).OrderByDescending(msg => msg.CreatedAt).FirstOrDefault();
            var unread  = m.Chat?.Messages.Count(msg => !msg.IsDeleted && msg.SenderId != userId && msg.ReadAt == null) ?? 0;
            return new MatchListItemDto
            {
                MatchId = m.Id.ToString(), ChatId = m.Chat?.Id.ToString(),
                MatchedAt = m.CreatedAt, UnreadCount = unread,
                MatchedUser = new MatchedUserDto { Id = other?.Id.ToString(), FullName = other?.FullName, Avatar = other?.Avatar },
                LastMessage = lastMsg == null ? null : new LastMessageDto { Text = lastMsg.Text, SentAt = lastMsg.CreatedAt }
            };
        }).ToList();
    }

    public async Task UnmatchAsync(Guid userId, Guid matchId)
    {
        var match = await _db.Matches.FirstOrDefaultAsync(m =>
            m.Id == matchId && (m.User1Id == userId || m.User2Id == userId))
            ?? throw new InvalidOperationException("Match not found.");
        match.IsActive = false; match.IsDeleted = true; match.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private static double? CalculateDistance(UserLocation? loc1, UserLocation? loc2)
    {
        if (loc1 == null || loc2 == null || !loc1.Lat.HasValue || !loc2.Lat.HasValue) return null;
        const double R = 6371;
        var dLat = ToRad(loc2.Lat.Value - loc1.Lat.Value);
        var dLon = ToRad(loc2.Lng!.Value - loc1.Lng!.Value);
        var a = Math.Sin(dLat/2)*Math.Sin(dLat/2) + Math.Cos(ToRad(loc1.Lat.Value))*Math.Cos(ToRad(loc2.Lat.Value))*Math.Sin(dLon/2)*Math.Sin(dLon/2);
        return Math.Round(R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a)), 1);
    }
    private static double ToRad(double deg) => deg * Math.PI / 180;
    private static int CalculateMatchScore(User me, User other)
    {
        var myI = me.Interests.Select(i => i.InterestId).ToHashSet();
        var thI = other.Interests.Select(i => i.InterestId).ToHashSet();
        return Math.Min(99, 50 + (myI.Intersect(thI).Count() * 10));
    }
}
