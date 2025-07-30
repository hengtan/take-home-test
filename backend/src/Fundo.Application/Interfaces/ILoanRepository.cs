using Fundo.Domain.Entities;

namespace Fundo.Application.Interfaces;

public interface ILoanRepository
{
    Task AddAsync(Loan loan, CancellationToken cancellationToken);
    Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Loan>> GetAllAsync(CancellationToken cancellationToken);
}