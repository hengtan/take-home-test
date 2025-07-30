using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fundo.Infrastructure.Persistence;

public class LoanDbContextFactory : IDesignTimeDbContextFactory<LoanDbContext>
{
    public LoanDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("Default");

        var optionsBuilder = new DbContextOptionsBuilder<LoanDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new LoanDbContext(optionsBuilder.Options);
    }
}