using Microsoft.EntityFrameworkCore;
using Mingley.Application.DTOs.Wallet;
using Mingley.Application.Interfaces;
using Mingley.Domain.Entities;
using Mingley.Infrastructure.Persistence;

namespace Mingley.Infrastructure.Services;

public class WalletService : IWalletService
{
    private readonly MingleyDbContext _db;
    public WalletService(MingleyDbContext db) => _db = db;

    public async Task<WalletBalanceDto> GetBalanceAsync(Guid userId)
    {
        var user = await _db.Users.FindAsync(userId);
        return new WalletBalanceDto { CoinBalance = user?.CoinBalance ?? 0 };
    }

    public Task<List<CoinPackageDto>> GetPackagesAsync() => Task.FromResult(new List<CoinPackageDto>
    {
        new() { Id = "pkg1", Coins = 100, Price = 49, Label = "Starter", IsPopular = false },
        new() { Id = "pkg2", Coins = 300, Price = 129, Label = "Popular", IsPopular = true },
        new() { Id = "pkg3", Coins = 700, Price = 249, Label = "Value", IsPopular = false },
        new() { Id = "pkg4", Coins = 1500, Price = 499, Label = "Pro", IsPopular = false },
    });

    public async Task<List<CoinTransactionDto>> GetTransactionsAsync(Guid userId, string type)
    {
        var query = _db.CoinTransactions.Where(t => t.UserId == userId);
        if (type != "all") query = query.Where(t => t.Direction == type);

        var txns = await query.OrderByDescending(t => t.CreatedAt).Take(50).ToListAsync();
        return txns.Select(t => new CoinTransactionDto
        {
            Id = t.Id.ToString(),
            Coins = t.Coins,
            Direction = t.Direction,
            Description = t.Description,
            TransactionType = t.TransactionType,
            CreatedAt = t.CreatedAt
        }).ToList();
    }

    public async Task SubmitDepositAsync(Guid userId, DepositRequestDto req)
    {
        _db.DepositRequests.Add(new DepositRequest
        {
            UserId = userId,
            UtrId = req.UtrId,
            ScreenshotUrl = req.ScreenshotUrl,
            RequestedCoins = req.RequestedCoins,
            Status = "pending"
        });
        await _db.SaveChangesAsync();
    }

    public async Task SubmitWithdrawalAsync(Guid userId, WithdrawalRequestDto req)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        if (user.Gender != "female")
            throw new InvalidOperationException("Only female users can withdraw.");

        if (user.CoinBalance < req.Coins)
            throw new InvalidOperationException("Insufficient coins.");

        user.CoinBalance -= req.Coins;
        user.UpdatedAt = DateTime.UtcNow;

        _db.WithdrawalRequests.Add(new WithdrawalRequest
        {
            UserId = userId,
            Coins = req.Coins,
            BankOrUpi = req.BankOrUpi,
            Status = "pending"
        });

        _db.CoinTransactions.Add(new CoinTransaction
        {
            UserId = userId,
            Coins = req.Coins,
            Direction = "debit",
            Description = "Withdrawal request",
            TransactionType = "withdrawal"
        });

        await _db.SaveChangesAsync();
    }

    public async Task AddCoinsAsync(Guid userId, int coins, string description, string transactionType)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        user.CoinBalance += coins;
        user.UpdatedAt = DateTime.UtcNow;

        _db.CoinTransactions.Add(new CoinTransaction
        {
            UserId = userId, Coins = coins, Direction = "credit",
            Description = description, TransactionType = transactionType
        });

        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeductCoinsAsync(Guid userId, int coins, string description, string transactionType)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null || user.CoinBalance < coins) return false;

        user.CoinBalance -= coins;
        user.UpdatedAt = DateTime.UtcNow;

        _db.CoinTransactions.Add(new CoinTransaction
        {
            UserId = userId, Coins = coins, Direction = "debit",
            Description = description, TransactionType = transactionType
        });

        await _db.SaveChangesAsync();
        return true;
    }
}
