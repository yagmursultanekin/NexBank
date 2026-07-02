using Microsoft.AspNetCore.Mvc;
using NexBank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace NexBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("my-accounts")]
    public async Task<IActionResult> GetMyAccounts()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
        return Ok(accounts);
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<IActionResult> GetTransactions(int accountId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var transactions = await _accountService.GetTransactionsByAccountIdAsync(accountId, startDate, endDate);
        return Ok(transactions);
    }
}
