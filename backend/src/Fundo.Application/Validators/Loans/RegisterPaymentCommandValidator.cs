using FluentValidation;
using Fundo.Application.Commands.Loans.RegisterPayment;
using Fundo.Application.Validators.Common;

namespace Fundo.Application.Validators.Loans;


public class RegisterPaymentCommandValidator : AbstractValidator<RegisterPaymentCommand>
{
    public RegisterPaymentCommandValidator()
    {
        RuleFor(x => x.LoanId).MustHaveValidLoanId();
        RuleFor(x => x.Amount).MustBePositivePayment();
    }
}