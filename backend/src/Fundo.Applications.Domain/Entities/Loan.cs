using Fundo.Applications.Domain.Enums;

namespace Fundo.Applications.Domain.Entities;

public class Loan : BaseEntity
{
    public decimal Amount { get; set; }
    public decimal CurrentBalance { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public LoanStatus Status { get; set; }
    public List<LoanPayment>? Payments { get; set; }
}
