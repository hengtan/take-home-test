namespace Fundo.Application.Interfaces;

public interface IUnitOfWork
{
    ILoanRepository LoanRepository { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken);
}