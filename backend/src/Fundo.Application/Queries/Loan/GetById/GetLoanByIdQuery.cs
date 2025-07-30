using MediatR;

namespace Fundo.Application.Queries.Loan.GetById;

public record GetLoanByIdQuery(Guid Id) : IRequest<LoanDetailsDto>;