using Fundo.Application.Common.Results;
using MediatR;
using Unit = Fundo.Application.Common.Results.Unit;

namespace Fundo.Application.Commands.Loans.Payment;

public abstract class AddPaymentCommand : IRequest<Unit>, IRequest<Result<Unit>>
{
    public Guid LoanId { get; init; }
    public decimal Amount { get; init; }
}