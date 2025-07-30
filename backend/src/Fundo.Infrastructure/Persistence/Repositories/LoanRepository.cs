using Fundo.Application.Interfaces;
using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Infrastructure.Persistence.Repositories;

public class LoanRepository(LoanDbContext context) : ILoanRepository
{
    public async Task AddAsync(Loan loan, CancellationToken cancellationToken)
    {
        await context.Loans.AddAsync(loan, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Loans
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Loan>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Loans
        .AsNoTracking()
        .OrderBy(l => l.ApplicantName)
        .ToListAsync(cancellationToken);
    }
}