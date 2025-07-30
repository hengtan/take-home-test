namespace Fundo.Domain.Errors;

public static class ValidationMessages
{
    public const string Required = "{PropertyName} is required.";
    public const string GreaterThanZero = "{PropertyName} must be greater than zero.";
    public const string GreaterOrEqualZero = "{PropertyName} must be zero or more.";
    public const string MaxLength = "{PropertyName} must not exceed {MaxLength} characters.";
    public const string CurrentBalanceCannotExceedAmount = "Current balance cannot exceed the loan amount.";
    public const string LoanIdRequired = "Loan ID is required.";
    public const string PaymentAmountMustBePositive = "Payment amount must be greater than zero.";
}