using Fundo.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fundo.Application.Queries.Loan.List;

public class GetAllLoansQueryHandler(ILoanRepository loanRepository, ILogger<GetAllLoansQueryHandler> logger)
    : IRequestHandler<GetAllLoansQuery, List<LoanListItemDto>>
{
    public async Task<List<LoanListItemDto>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all loans");

        var loans = await loanRepository.GetAllAsync(cancellationToken);

        var dtos = loans.OrderBy(a=>a.ApplicantName)
            .Select(MapToListItemDto).ToList();

        logger.LogInformation("{Count} loans fetched successfully", dtos.Count);

        return dtos;
    }

    private static LoanListItemDto MapToListItemDto(Domain.Entities.Loan loan)
    {
        return new LoanListItemDto
        {
            Id = loan.Id,
            ApplicantName = loan.ApplicantName,
            CurrentBalance = loan.CurrentBalance,
            Amount = loan.Amount,
            Status = loan.Status.ToString("G")
        };
    }
}