using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace F_ckCreditSlavery.Entities;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("sqlConnection"));

        return new RepositoryContext(optionsBuilder.Options);
    }
}