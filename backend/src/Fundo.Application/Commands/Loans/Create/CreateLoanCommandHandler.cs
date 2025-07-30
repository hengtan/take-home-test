using Fundo.Application.Common.Errors;
using Fundo.Application.Common.Results;
using Fundo.Application.Errors.ErrorsMessages;
using Fundo.Application.Interfaces;
using Fundo.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Unit = Fundo.Application.Common.Results.Unit;

namespace Fundo.Application.Commands.Loans.Create;

public class CreateLoanCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateLoanCommandHandler> logger)
    : IRequestHandler<CreateLoanCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting loan creation for applicant: {Applicant}", request.ApplicantName);

        if (IsInvalid(request))
        {
            logger.LogWarning("Invalid loan request for applicant: {Applicant}", request.ApplicantName);
            return Result<Guid>.Failure(Error.Validation(ErrorMessages.LoanAmountAndBalanceMustBePositive));
        }

        var loan = CreateLoanEntity(request);

        var saveResult = await PersistLoanAsync(loan, cancellationToken);
        if (saveResult.IsFailure)
        {
            return Result<Guid>.Failure(saveResult.Error!);
        }

        logger.LogInformation("Loan created successfully for applicant: {Applicant}, LoanId: {LoanId}",
            loan.ApplicantName, loan.Id);

        return Result<Guid>.Success(loan.Id);
    }

    private static bool IsInvalid(CreateLoanCommand request)
    {
        return request.Amount <= 0 || request.CurrentBalance < 0;
    }

    private static Loan CreateLoanEntity(CreateLoanCommand request)
    {
        return Loan.Create(request.Amount, request.CurrentBalance, request.ApplicantName);
    }

    private async Task<Result<Unit>> PersistLoanAsync(Loan loan, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.LoanRepository.AddAsync(loan, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to persist loan for applicant: {Applicant}", loan.ApplicantName);
            return Result<Unit>.Failure(Error.Internal(ErrorMessages.LoanSaveInternalError));
        }
    }
}