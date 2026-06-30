using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NexBank.Domain.Entities;

namespace NexBank.Infrastructure.Persistence;

public class NexBankDbContext : DbContext
{
    public NexBankDbContext(DbContextOptions<NexBankDbContext> options) : base(options)
    {
    }

    public DbSet <User> Users { get; set; }
    public DbSet <Account> Accounts { get; set; }
    public DbSet <Transaction> Transactions { get; set; }
}
