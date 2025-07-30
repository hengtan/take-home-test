using FluentValidation;
using Fundo.Application.Commands.Loans.Payment;
using Fundo.Application.Validators.Common;

namespace Fundo.Application.Validators.Loans;

public class AddPaymentCommandValidator : AbstractValidator<AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(x => x.LoanId).MustHaveValidLoanId();
        RuleFor(x => x.Amount).MustBePositivePayment();
    }
}