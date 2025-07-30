using FluentValidation;
using Fundo.Domain.Errors;

namespace Fundo.Application.Validators.Common;

public static class CustomRuleBuilderExtensions
{
    public static IRuleBuilder<T, Guid> MustHaveValidLoanId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(ValidationMessages.LoanIdRequired);
    }

    public static IRuleBuilder<T, decimal> MustBePositivePayment<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PaymentAmountMustBePositive);
    }
}