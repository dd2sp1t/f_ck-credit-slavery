using F_ckCreditSlavery.Entities.Configurations;
using F_ckCreditSlavery.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace F_ckCreditSlavery.Entities;

public class RepositoryContext : IdentityDbContext<User>
{
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new CreditAccountConfiguration());
        modelBuilder.ApplyConfiguration(new CreditAccountChangeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }

    // public DbSet<User> Users { get; set; } = null!;
    public DbSet<CreditAccount> CreditAccounts { get; set; } = null!;
    public DbSet<CreditAccountChange> CreditAccountChanges { get; set; } = null!;
}