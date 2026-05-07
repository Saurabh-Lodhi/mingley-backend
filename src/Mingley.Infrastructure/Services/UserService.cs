using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Users;
using Mingley.Application.Interfaces;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;

namespace Mingley.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly MingleyDbContext _db;
    private readonly IMapper _mapper;

    public UserService(MingleyDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<UserProfileDto?> GetMeAsync(Guid userId)
    {
        var user = await _db.Users
            .Include(u => u.Location)
            .Include(u => u.Preference)
            .Include(u => u.Images)
            .Include(u => u.Interests).ThenInclude(i => i.Interest)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        return user == null ? null : _mapper.Map<UserProfileDto>(user);
    }

    public async Task<UserProfileDto?> GetUserAsync(Guid userId, Guid requesterId)
    {
        // Check if requester has blocked or been blocked
        var blocked = await _db.Blocks.AnyAsync(b =>
            (b.BlockerId == requesterId && b.BlockedUserId == userId) ||
            (b.BlockerId == userId && b.BlockedUserId == requesterId));

        if (blocked) return null;

        var user = await _db.Users
            .Include(u => u.Location)
            .Include(u => u.Images)
            .Include(u => u.Interests).ThenInclude(i => i.Interest)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted && u.IsActive);

        return user == null ? null : _mapper.Map<UserProfileDto>(user);
    }

    public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest req)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        if (req.FullName != null) user.FullName = req.FullName;
        if (req.Bio != null) user.Bio = req.Bio;
        if (req.Gender != null) user.Gender = req.Gender;
        if (req.DateOfBirth.HasValue) user.DateOfBirth = req.DateOfBirth.Value.ToUniversalTime();
        if (req.Avatar != null) user.Avatar = req.Avatar;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (await GetMeAsync(userId))!;
    }

    public async Task UpdateInterestsAsync(Guid userId, List<string> interests)
    {
        // Remove old interests
        var old = await _db.UserInterests.Where(ui => ui.UserId == userId).ToListAsync();
        _db.UserInterests.RemoveRange(old);

        // Add new ones
        foreach (var name in interests.Distinct())
        {
            var interest = await _db.Interests.FirstOrDefaultAsync(i => i.Name == name);
            if (interest == null)
            {
                interest = new Interest { Name = name };
                _db.Interests.Add(interest);
                await _db.SaveChangesAsync();
            }
            _db.UserInterests.Add(new UserInterest { UserId = userId, InterestId = interest.Id });
        }
        await _db.SaveChangesAsync();
    }

    public async Task UpdatePreferencesAsync(Guid userId, UpdatePreferencesRequest req)
    {
        var pref = await _db.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
        if (pref == null)
        {
            pref = new UserPreference { UserId = userId };
            _db.UserPreferences.Add(pref);
        }

        if (req.InterestedIn != null) pref.InterestedIn = req.InterestedIn;
        if (req.MinAge.HasValue) pref.MinAge = req.MinAge;
        if (req.MaxAge.HasValue) pref.MaxAge = req.MaxAge;
        if (req.MaxDistance.HasValue) pref.MaxDistance = req.MaxDistance;
        if (req.RelationshipType != null) pref.RelationshipType = req.RelationshipType;
        if (req.NearbyOnly.HasValue) pref.NearbyOnly = req.NearbyOnly;
        if (req.OnlineOnly.HasValue) pref.OnlineOnly = req.OnlineOnly;
        if (req.VerifiedOnly.HasValue) pref.VerifiedOnly = req.VerifiedOnly;
        if (req.Location != null) pref.Location = req.Location;
        pref.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task UpdateLocationAsync(Guid userId, UpdateLocationRequest req)
    {
        var loc = await _db.UserLocations.FirstOrDefaultAsync(l => l.UserId == userId);
        if (loc == null) { loc = new UserLocation { UserId = userId }; _db.UserLocations.Add(loc); }

        if (req.Lat.HasValue) loc.Lat = req.Lat;
        if (req.Lng.HasValue) loc.Lng = req.Lng;
        if (req.City != null) loc.City = req.City;
        if (req.Country != null) loc.Country = req.Country;
        loc.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task AddImageAsync(Guid userId, string url)
    {
        var count = await _db.UserImages.CountAsync(i => i.UserId == userId && !i.IsDeleted);
        _db.UserImages.Add(new UserImage { UserId = userId, Url = url, SortOrder = count });
        await _db.SaveChangesAsync();
    }

    public async Task DeleteImageAsync(Guid userId, Guid imageId)
    {
        var image = await _db.UserImages.FirstOrDefaultAsync(i => i.Id == imageId && i.UserId == userId)
            ?? throw new InvalidOperationException("Image not found.");
        image.IsDeleted = true;
        image.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task BlockUserAsync(Guid blockerId, Guid targetId)
    {
        var exists = await _db.Blocks.AnyAsync(b => b.BlockerId == blockerId && b.BlockedUserId == targetId);
        if (!exists) _db.Blocks.Add(new Block { BlockerId = blockerId, BlockedUserId = targetId });
        await _db.SaveChangesAsync();
    }

    public async Task UnblockUserAsync(Guid blockerId, Guid targetId)
    {
        var block = await _db.Blocks.FirstOrDefaultAsync(b => b.BlockerId == blockerId && b.BlockedUserId == targetId);
        if (block != null) { _db.Blocks.Remove(block); await _db.SaveChangesAsync(); }
    }

    public async Task<List<UserProfileDto>> GetBlockedUsersAsync(Guid userId)
    {
        var blocked = await _db.Blocks
            .Include(b => b.BlockedUser).ThenInclude(u => u!.Location)
            .Where(b => b.BlockerId == userId)
            .Select(b => b.BlockedUser!)
            .ToListAsync();
        return _mapper.Map<List<UserProfileDto>>(blocked);
    }
}
