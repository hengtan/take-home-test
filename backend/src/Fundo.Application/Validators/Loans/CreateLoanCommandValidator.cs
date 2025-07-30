using FluentValidation;
using Fundo.Application.Commands.Loans.Create;
using Fundo.Application.Validators.Common;
using Fundo.Domain.Errors;

namespace Fundo.Application.Validators.Loans;

public abstract class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    protected CreateLoanCommandValidator()
    {
        RuleFor(x => x.Amount)
            .MustBePositive();

        RuleFor(x => x.CurrentBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ValidationMessages.GreaterOrEqualZero)
            .LessThanOrEqualTo(x => x.Amount)
            .WithMessage(ValidationMessages.CurrentBalanceCannotExceedAmount);

        RuleFor(x => x.ApplicantName)
            .RequiredWithMaxLength(100);
    }
}