namespace Fundo.Application.Queries.Loan.List;
using MediatR;

public record GetAllLoansQuery : IRequest<List<LoanListItemDto>>;