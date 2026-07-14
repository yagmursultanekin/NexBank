using Microsoft.EntityFrameworkCore;
using UzayBank.Domain.Entities;

namespace UzayBank.Infrastructure.Persistence;

public class UzayBankDbContext : DbContext
{
    public UzayBankDbContext(DbContextOptions<UzayBankDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserAccount>(entity =>
        {
            // Aynı hesap aynı kullanıcıya iki kez bağlanamaz.
            // Bu kısıtı koda değil veritabanına yaptırıyoruz —
            // kod hata yapabilir, DB kısıtı yapamaz.
            entity.HasIndex(ua => new { ua.UserId, ua.AccountNumber }).IsUnique();

            entity.Property(ua => ua.AccountNumber).HasMaxLength(34).IsRequired();
            entity.Property(ua => ua.IBAN).HasMaxLength(34);
            entity.Property(ua => ua.Currency).HasMaxLength(3);

            // Kullanıcı silinirse eşlemeleri de silinir — yetim kayıt kalmaz.
            entity.HasOne(ua => ua.User)
                  .WithMany()
                  .HasForeignKey(ua => ua.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}