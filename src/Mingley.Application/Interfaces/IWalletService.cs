using Mingley.Application.DTOs.Wallet;

namespace Mingley.Application.Interfaces;

public interface IWalletService
{
    Task<WalletBalanceDto> GetBalanceAsync(Guid userId);
    Task<List<CoinPackageDto>> GetPackagesAsync();
    Task<List<CoinTransactionDto>> GetTransactionsAsync(Guid userId, string type);
    Task SubmitDepositAsync(Guid userId, DepositRequestDto request);
    Task SubmitWithdrawalAsync(Guid userId, WithdrawalRequestDto request);
    Task AddCoinsAsync(Guid userId, int coins, string description, string transactionType);
    Task<bool> DeductCoinsAsync(Guid userId, int coins, string description, string transactionType);
}
