namespace Fundo.Applications.Domain.Entities;

public class LoanPayment : BaseEntity
{
    public decimal Amount { get; set; }
    public int LoanId { get; set; }
    public Loan Loan { get; set; } = default!;
}
