using System;
using Fundo.Application.Common.Results;
using MediatR;
using Unit = Fundo.Application.Common.Results.Unit;

namespace Fundo.Application.Commands.Loans.RegisterPayment;

public record RegisterPaymentCommand(Guid LoanId, decimal Amount) : IRequest<Result<Unit>>;