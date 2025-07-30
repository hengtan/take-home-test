using FluentValidation;

namespace Fundo.Application.Validators.Common;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, decimal> MustBePositive<T>(
        this IRuleBuilder<T, decimal> ruleBuilder) =>
        ruleBuilder.GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

    public static IRuleBuilderOptions<T, string> RequiredWithMaxLength<T>(
        this IRuleBuilder<T, string> ruleBuilder, int maxLength) =>
        ruleBuilder
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(maxLength).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");
}