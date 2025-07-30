using Fundo.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fundo.Application.Queries.Loan.GetById;

public class GetLoanByIdQueryHandler(ILoanRepository loanRepository, ILogger<GetLoanByIdQueryHandler> logger)
    : IRequestHandler<GetLoanByIdQuery, LoanDetailsDto?>
{
    public async Task<LoanDetailsDto?> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving loan with ID: {LoanId}", request.Id);

        var loan = await loanRepository.GetByIdAsync(request.Id, cancellationToken);

        if (loan is not null) return MapToDto(loan);
        logger.LogWarning("Loan not found: {LoanId}", request.Id);
        return null;

    }

    private static LoanDetailsDto MapToDto(Domain.Entities.Loan loan)
    {
        return new LoanDetailsDto
        {
            Id = loan.Id,
            Amount = loan.Amount,
            CurrentBalance = loan.CurrentBalance,
            ApplicantName = loan.ApplicantName,
            Status = loan.Status.ToString("G")
        };
    }
}