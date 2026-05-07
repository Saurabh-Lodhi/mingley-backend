using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Common;
using Mingley.Infrastructure.Persistence;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/notifications")]
[Authorize]
[Produces("application/json")]
public class NotificationsController : ControllerBase
{
    private readonly MingleyDbContext _db;
    public NotificationsController(MingleyDbContext db) => _db = db;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await _db.Notifications
            .Where(n => n.UserId == CurrentUserId && !n.IsDeleted)
            .OrderByDescending(n => n.CreatedAt).Take(50)
            .Select(n => new { id = n.Id.ToString(), n.Title, n.Body, n.Type, n.IsRead, n.CreatedAt, n.ReferenceId })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(new { notifications }));
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCount()
    {
        var count = await _db.Notifications
            .CountAsync(n => n.UserId == CurrentUserId && !n.IsRead && !n.IsDeleted);
        return Ok(ApiResponse<object>.Ok(new { count }));
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkRead(Guid id)
    {
        var n = await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == CurrentUserId);
        if (n != null) { n.IsRead = true; await _db.SaveChangesAsync(); }
        return Ok(ApiResponse.Ok("Marked as read."));
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllRead()
    {
        var unread = await _db.Notifications
            .Where(n => n.UserId == CurrentUserId && !n.IsRead).ToListAsync();
        unread.ForEach(n => n.IsRead = true);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("All notifications marked as read."));
    }
}
