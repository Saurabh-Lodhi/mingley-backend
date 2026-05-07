using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Common;
using Mingley.Infrastructure.Persistence;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/interests")]
[Produces("application/json")]
public class InterestsController : ControllerBase
{
    private readonly MingleyDbContext _db;
    public InterestsController(MingleyDbContext db) => _db = db;

    /// <summary>Get all interests — frontend loads from here instead of hardcoded</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var interests = await _db.Interests
            .Where(i => !i.IsDeleted)
            .OrderBy(i => i.Name)
            .Select(i => new { id = i.Id.ToString(), i.Name, i.Icon })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(new { interests }));
    }
}
