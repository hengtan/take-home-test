using Fundo.Application.Interfaces;

namespace Fundo.Infrastructure.Persistence;

public class UnitOfWork(LoanDbContext context, ILoanRepository loanRepository) : IUnitOfWork
{
    public ILoanRepository LoanRepository => loanRepository;

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}