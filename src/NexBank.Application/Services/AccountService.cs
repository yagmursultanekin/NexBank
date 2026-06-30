using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NexBank.Application.DTOs;
using NexBank.Application.Interfaces;
using NexBank.Domain.Interfaces;

namespace NexBank.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<List<AccountDto>> GetAccountsByUserIdAsync(int userId)
    {
        var accounts = await _accountRepository.GetByUserIdAsync(userId);
        return _mapper.Map<List<AccountDto>>(accounts);
    }

    public async Task<AccountDto?> GetAccountByIdAsync(int accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        return account == null ? null : _mapper.Map<AccountDto>(account);
    }

    public async Task<List<TransactionDto>> GetTransactionsByAccountIdAsync(int accountId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _accountRepository.GetTransactionsAsync(accountId, startDate, endDate);
        return _mapper.Map<List<TransactionDto>>(transactions);
    }
}
