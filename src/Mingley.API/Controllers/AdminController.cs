using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Common;
using Mingley.Infrastructure.Persistence;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/admin")]
[Authorize(Roles = "admin")]
[Produces("application/json")]
public class AdminController : ControllerBase
{
    private readonly MingleyDbContext _db;
    public AdminController(MingleyDbContext db) => _db = db;

    /// <summary>Dashboard stats</summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var totalUsers    = await _db.Users.CountAsync();
        var totalMatches  = await _db.Matches.CountAsync();
        var totalMessages = await _db.Messages.CountAsync();
        var pendingDeposits    = await _db.DepositRequests.CountAsync(d => d.Status == "pending");
        var pendingWithdrawals = await _db.WithdrawalRequests.CountAsync(w => w.Status == "pending");
        var premiumUsers  = await _db.Users.CountAsync(u => u.IsPremium);
        var onlineUsers   = await _db.Users.CountAsync(u => u.IsOnline);

        return Ok(ApiResponse<object>.Ok(new {
            totalUsers, totalMatches, totalMessages,
            pendingDeposits, pendingWithdrawals,
            premiumUsers, onlineUsers
        }));
    }

    /// <summary>Get all users</summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 20, [FromQuery] string? search = null)
    {
        var query = _db.Users.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.FullName!.Contains(search) || u.Email!.Contains(search));

        var total = await query.CountAsync();
        var users = await query.OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * limit).Take(limit)
            .Select(u => new {
                u.Id, u.FullName, u.Email, u.Phone, u.Gender,
                u.IsVerified, u.IsPremium, u.IsActive, u.CoinBalance,
                u.Role, u.CreatedAt, u.LastActiveAt
            }).ToListAsync();

        return Ok(ApiResponse<object>.Ok(new { users, total, page, limit }));
    }

    /// <summary>Toggle user active/suspended</summary>
    [HttpPut("users/{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var user = await _db.Users.FindAsync(id) ?? throw new InvalidOperationException("User not found.");
        user.IsActive = !user.IsActive;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok($"User {(user.IsActive ? "activated" : "suspended")}."));
    }

    /// <summary>Get pending deposit requests</summary>
    [HttpGet("deposits")]
    public async Task<IActionResult> GetDeposits([FromQuery] string status = "pending")
    {
        var deposits = await _db.DepositRequests
            .Include(d => d.User)
            .Where(d => d.Status == status)
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new {
                d.Id, d.UtrId, d.RequestedCoins, d.Status, d.CreatedAt,
                d.ScreenshotUrl, d.AdminNote,
                User = new { d.User!.FullName, d.User.Email }
            }).ToListAsync();
        return Ok(ApiResponse<object>.Ok(new { deposits }));
    }

    /// <summary>Approve deposit — adds coins to user</summary>
    [HttpPost("deposits/{id}/approve")]
    public async Task<IActionResult> ApproveDeposit(Guid id, [FromBody] AdminNoteRequest req)
    {
        var deposit = await _db.DepositRequests.Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id && d.Status == "pending")
            ?? throw new InvalidOperationException("Deposit not found or already processed.");

        deposit.Status = "approved";
        deposit.AdminNote = req.Note;

        // Add coins to user
        deposit.User!.CoinBalance += deposit.RequestedCoins ?? 0;
        _db.CoinTransactions.Add(new Domain.Entities.CoinTransaction
        {
            UserId = deposit.UserId,
            Coins = deposit.RequestedCoins ?? 0,
            Direction = "credit",
            Description = $"Deposit approved (UTR: {deposit.UtrId})",
            TransactionType = "deposit"
        });

        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok($"{deposit.RequestedCoins} coins added to {deposit.User.FullName}."));
    }

    /// <summary>Reject deposit</summary>
    [HttpPost("deposits/{id}/reject")]
    public async Task<IActionResult> RejectDeposit(Guid id, [FromBody] AdminNoteRequest req)
    {
        var deposit = await _db.DepositRequests
            .FirstOrDefaultAsync(d => d.Id == id && d.Status == "pending")
            ?? throw new InvalidOperationException("Deposit not found.");
        deposit.Status = "rejected";
        deposit.AdminNote = req.Note;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("Deposit rejected."));
    }

    /// <summary>Get pending withdrawal requests</summary>
    [HttpGet("withdrawals")]
    public async Task<IActionResult> GetWithdrawals([FromQuery] string status = "pending")
    {
        var withdrawals = await _db.WithdrawalRequests
            .Include(w => w.User)
            .Where(w => w.Status == status)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new {
                w.Id, w.Coins, w.BankOrUpi, w.Status, w.CreatedAt, w.AdminNote,
                User = new { w.User!.FullName, w.User.Email, w.User.CoinBalance }
            }).ToListAsync();
        return Ok(ApiResponse<object>.Ok(new { withdrawals }));
    }

    /// <summary>Approve withdrawal</summary>
    [HttpPost("withdrawals/{id}/approve")]
    public async Task<IActionResult> ApproveWithdrawal(Guid id, [FromBody] AdminNoteRequest req)
    {
        var wr = await _db.WithdrawalRequests
            .FirstOrDefaultAsync(w => w.Id == id && w.Status == "pending")
            ?? throw new InvalidOperationException("Withdrawal not found.");
        wr.Status = "approved";
        wr.AdminNote = req.Note;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok("Withdrawal approved. Process the payment manually."));
    }

    /// <summary>Reject withdrawal — refund coins</summary>
    [HttpPost("withdrawals/{id}/reject")]
    public async Task<IActionResult> RejectWithdrawal(Guid id, [FromBody] AdminNoteRequest req)
    {
        var wr = await _db.WithdrawalRequests.Include(w => w.User)
            .FirstOrDefaultAsync(w => w.Id == id && w.Status == "pending")
            ?? throw new InvalidOperationException("Withdrawal not found.");
        wr.Status = "rejected";
        wr.AdminNote = req.Note;
        // Refund coins
        wr.User!.CoinBalance += wr.Coins;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok($"{wr.Coins} coins refunded to {wr.User.FullName}."));
    }

    /// <summary>Get all reports</summary>
    [HttpGet("reports")]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _db.Reports
            .Include(r => r.Reporter)
            .Include(r => r.ReportedUser)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new {
                r.Id, r.Reason, r.Status, r.CreatedAt,
                Reporter = new { r.Reporter!.FullName, r.Reporter.Email },
                ReportedUser = new { r.ReportedUser!.FullName, r.ReportedUser.Email }
            }).ToListAsync();
        return Ok(ApiResponse<object>.Ok(new { reports }));
    }

    /// <summary>Manually add coins to a user</summary>
    [HttpPost("users/{id}/add-coins")]
    public async Task<IActionResult> AddCoins(Guid id, [FromBody] AddCoinsRequest req)
    {
        var user = await _db.Users.FindAsync(id) ?? throw new InvalidOperationException("User not found.");
        user.CoinBalance += req.Coins;
        _db.CoinTransactions.Add(new Domain.Entities.CoinTransaction
        {
            UserId = id, Coins = req.Coins, Direction = "credit",
            Description = req.Note ?? "Admin manual top-up", TransactionType = "admin"
        });
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok($"{req.Coins} coins added. New balance: {user.CoinBalance}"));
    }
}

public class AdminNoteRequest { public string? Note { get; set; } }
public class AddCoinsRequest { public int Coins { get; set; } public string? Note { get; set; } }
