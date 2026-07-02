using Microsoft.AspNetCore.Mvc;
using NexBank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;


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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAccountsByUserId(int userId)
    {
        var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
        return Ok(accounts);
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAccountById(int accountId)
    {
        var account = await _accountService.GetAccountByIdAsync(accountId);
        if (account == null)
            return NotFound();
        return Ok(account);
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<IActionResult> GetTransactions(int accountId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var transactions = await _accountService.GetTransactionsByAccountIdAsync(accountId, startDate, endDate);
        return Ok(transactions);
    }
}
