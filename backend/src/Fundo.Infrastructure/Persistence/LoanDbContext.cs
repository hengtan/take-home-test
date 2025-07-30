using Fundo.Domain.Entities;
using Fundo.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Infrastructure.Persistence;

public class LoanDbContext(DbContextOptions<LoanDbContext> options) : DbContext(options)
{
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LoanConfiguration());
    }
}