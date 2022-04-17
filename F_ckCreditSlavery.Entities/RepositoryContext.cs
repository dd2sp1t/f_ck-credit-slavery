using F_ckCreditSlavery.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace F_ckCreditSlavery.Entities;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }
        
    public DbSet<CreditAccount> CreditAccounts { get; set; } = null!;
    public DbSet<CreditAccountChange> CreditAccountChanges { get; set; } = null!;
}