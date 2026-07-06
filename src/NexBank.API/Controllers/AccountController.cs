using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexBank.Application.DTOs;
using NexBank.Application.Interfaces;
using System.Security.Claims;

namespace NexBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
//localhost:4000/api/Account/my-accounts

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return claim == null ? null : int.Parse(claim);
    }

    [HttpGet("my-accounts")]
    public async Task<IActionResult> GetMyAccounts()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var accounts = await _accountService.GetAccountsByUserIdAsync(userId.Value);
        return Ok(accounts);
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAccountById(int accountId)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var accounts = await _accountService.GetAccountsByUserIdAsync(userId.Value);
        return Ok(accounts);
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<IActionResult> GetTransactions(int accountId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var accounts = await _accountService.GetAccountsByUserIdAsync(userId.Value);
        return Ok(accounts);
    }
    [HttpPost("{accountId}/transactions")]
    public async Task<IActionResult> AddTransaction(int accountId, CreateTransactionDto dto)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var accounts = await _accountService.GetAccountsByUserIdAsync(userId.Value);
        return Ok(accounts);
    }
}