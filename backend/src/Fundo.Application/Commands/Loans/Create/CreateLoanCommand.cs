using Fundo.Application.Common.Results;
using MediatR;

namespace Fundo.Application.Commands.Loans.Create;

public record CreateLoanCommand(
    decimal Amount,
    decimal CurrentBalance,
    string ApplicantName
) : IRequest<Result<Guid>>;