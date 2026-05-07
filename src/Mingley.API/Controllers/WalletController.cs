using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mingley.Application.DTOs.Common;
using Mingley.Application.DTOs.Wallet;
using Mingley.Application.Interfaces;
using System.Security.Claims;

namespace Mingley.API.Controllers;

[ApiController]
[Route("v1/wallet")]
[Authorize]
[Produces("application/json")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _wallet;
    public WalletController(IWalletService wallet) => _wallet = wallet;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get coin balance — matches ProfileScreen.js wallet display</summary>
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var balance = await _wallet.GetBalanceAsync(CurrentUserId);
        return Ok(ApiResponse<WalletBalanceDto>.Ok(balance));
    }

    /// <summary>Get coin packages for purchase</summary>
    [HttpGet("packages")]
    public async Task<IActionResult> GetPackages()
    {
        var packages = await _wallet.GetPackagesAsync();
        return Ok(ApiResponse<List<CoinPackageDto>>.Ok(packages));
    }

    /// <summary>Get transaction history — matches useChatStore.js transactions</summary>
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions([FromQuery] string type = "all")
    {
        var txns = await _wallet.GetTransactionsAsync(CurrentUserId, type);
        return Ok(ApiResponse<object>.Ok(new { transactions = txns }));
    }

    /// <summary>Submit deposit request (UTR ID) — matches DepositModal.js</summary>
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequestDto req)
    {
        await _wallet.SubmitDepositAsync(CurrentUserId, req);
        return Ok(ApiResponse.Ok("Deposit request submitted. Coins will be added after verification."));
    }

    /// <summary>Submit withdrawal — matches CashoutModal.js (female users only)</summary>
    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawalRequestDto req)
    {
        try
        {
            await _wallet.SubmitWithdrawalAsync(CurrentUserId, req);
            return Ok(ApiResponse.Ok("Withdrawal request submitted."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }
}
