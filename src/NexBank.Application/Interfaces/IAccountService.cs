using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexBank.Application.DTOs;

namespace NexBank.Application.Interfaces;

public interface IAccountService
{
    Task<List<AccountDto>> GetAccountByUserIdAsync(int userId);
    Task<AccountDto?> GetAccountByIdAsync(int accountId);
    Task<List<TransactionDto>> GetTransactionsByAccountIdAsync(int accountId, DateTime startDate, DateTime endDate);
}
