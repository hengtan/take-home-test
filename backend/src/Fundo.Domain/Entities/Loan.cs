namespace Fundo.Domain.Entities;

public enum LoanStatus
{
    Active = 1,
    Paid = 2
}

public class Loan
{
    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public string ApplicantName { get; private set; } = string.Empty;
    public LoanStatus Status { get; private set; }

    public bool IsPaid => CurrentBalance <= 0;

    protected Loan() {}

    private Loan(decimal amount, decimal currentBalance, string applicantName)
    {
        if (amount <= 0)
            throw new ArgumentException("Loan amount must be greater than zero.");

        if (currentBalance < 0)
            throw new ArgumentException("Current balance cannot be negative.");

        if (currentBalance > amount)
            throw new ArgumentException("Current balance cannot exceed the loan amount.");

        if (string.IsNullOrWhiteSpace(applicantName))
            throw new ArgumentException("Applicant name is required.");

        Id = Guid.NewGuid();
        Amount = amount;
        CurrentBalance = currentBalance;
        ApplicantName = applicantName;
        Status = LoanStatus.Active;
        UpdateStatus();
    }

    public static Loan Create(decimal amount, decimal currentBalance, string applicantName)
    {
        return new Loan(amount, currentBalance, applicantName);
    }

    public void RegisterPayment(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be positive.");

        if (IsPaid)
            throw new InvalidOperationException("Loan is already paid.");

        CurrentBalance = Math.Max(0, CurrentBalance - amount);
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        Status = IsPaid ? LoanStatus.Paid : LoanStatus.Active;
    }
}