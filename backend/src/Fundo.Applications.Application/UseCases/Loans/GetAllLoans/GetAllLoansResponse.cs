using Fundo.Applications.Domain.Enums;

namespace Fundo.Applications.Application.UseCases.Loans.GetAllLoans;

/// <summary>
/// Represents the response for retrieving all loans.
/// </summary>
public sealed record GetAllLoansResponse(
    /// <summary>
    /// Loan unique identifier.
    /// </summary>
    int Id,
    /// <summary>
    /// The total amount of the loan.
    /// </summary>
    decimal Amount,
    /// <summary>
    /// The current balance remaining on the loan.
    /// </summary>
    decimal CurrentBalance,
    /// <summary>
    /// The name of the loan applicant.
    /// </summary>
    string ApplicantName,
    /// <summary>
    /// The current status of the loan.
    /// </summary>
    LoanStatus Status);