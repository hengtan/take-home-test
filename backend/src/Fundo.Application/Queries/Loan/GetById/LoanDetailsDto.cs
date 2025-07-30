namespace Fundo.Application.Queries.Loan.GetById;

public class LoanDetailsDto
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public decimal CurrentBalance { get; init; }
    public string ApplicantName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}