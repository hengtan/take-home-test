namespace Fundo.Application.Queries.Loan.List;

public class LoanListItemDto
{
    public Guid Id { get; init; }
    public string ApplicantName { get; init; } = string.Empty;
    public decimal CurrentBalance { get; init; }
    public decimal Amount { get; init; }
    public string Status { get; init; } = string.Empty;
}