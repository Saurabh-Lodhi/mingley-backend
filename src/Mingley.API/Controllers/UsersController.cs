using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mingley.Application.DTOs.Common;
using Mingley.Application.DTOs.Users;
using Mingley.Application.Interfaces;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/users")]
[Authorize]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _users;
    public UsersController(IUserService users) => _users = users;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get current user profile — used by ProfileScreen.js</summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var profile = await _users.GetMeAsync(CurrentUserId);
        return profile == null
            ? NotFound(ApiResponse<object>.Fail("User not found.", 404))
            : Ok(ApiResponse<UserProfileDto>.Ok(profile));
    }

    /// <summary>Get another user's profile — used by UserProfileScreen.js</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var profile = await _users.GetUserAsync(id, CurrentUserId);
        return profile == null
            ? NotFound(ApiResponse<object>.Fail("User not found.", 404))
            : Ok(ApiResponse<UserProfileDto>.Ok(profile));
    }

    /// <summary>Update my profile — name, bio, gender, DOB, avatar</summary>
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest req)
    {
        var profile = await _users.UpdateProfileAsync(CurrentUserId, req);
        return Ok(ApiResponse<UserProfileDto>.Ok(profile, "Profile updated."));
    }

    /// <summary>Update interests — matches InterestChips.js save</summary>
    [HttpPut("me/interests")]
    public async Task<IActionResult> UpdateInterests([FromBody] UpdateInterestsRequest req)
    {
        await _users.UpdateInterestsAsync(CurrentUserId, req.Interests);
        return Ok(ApiResponse.Ok("Interests updated."));
    }

    /// <summary>Update discovery preferences — matches FilterSheet.js</summary>
    [HttpPut("me/preferences")]
    public async Task<IActionResult> UpdatePreferences([FromBody] UpdatePreferencesRequest req)
    {
        await _users.UpdatePreferencesAsync(CurrentUserId, req);
        return Ok(ApiResponse.Ok("Preferences updated."));
    }

    /// <summary>Update location — matches app location tracking</summary>
    [HttpPut("me/location")]
    public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationRequest req)
    {
        await _users.UpdateLocationAsync(CurrentUserId, req);
        return Ok(ApiResponse.Ok("Location updated."));
    }

    /// <summary>Add photo URL — matches PhotoGrid.js add button</summary>
    [HttpPost("me/images")]
    public async Task<IActionResult> AddImage([FromBody] AddImageRequest req)
    {
        await _users.AddImageAsync(CurrentUserId, req.Url);
        return Ok(ApiResponse.Ok("Image added."));
    }

    /// <summary>Delete a photo</summary>
    [HttpDelete("me/images/{imageId}")]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        try
        {
            await _users.DeleteImageAsync(CurrentUserId, imageId);
            return Ok(ApiResponse.Ok("Image deleted."));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }

    /// <summary>Block a user</summary>
    [HttpPost("{id}/block")]
    public async Task<IActionResult> Block(Guid id)
    {
        await _users.BlockUserAsync(CurrentUserId, id);
        return Ok(ApiResponse.Ok("User blocked."));
    }

    /// <summary>Unblock a user</summary>
    [HttpDelete("{id}/block")]
    public async Task<IActionResult> Unblock(Guid id)
    {
        await _users.UnblockUserAsync(CurrentUserId, id);
        return Ok(ApiResponse.Ok("User unblocked."));
    }

    /// <summary>Get blocked users list — matches SettingsScreen.js Block List</summary>
    [HttpGet("blocked")]
    public async Task<IActionResult> GetBlocked()
    {
        var blocked = await _users.GetBlockedUsersAsync(CurrentUserId);
        return Ok(ApiResponse<List<UserProfileDto>>.Ok(blocked));
    }
}

public class AddImageRequest
{
    public string Url { get; set; } = string.Empty;
}
