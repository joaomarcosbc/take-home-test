using Fundo.Applications.Domain.Enums;

namespace Fundo.Applications.Application.UseCases.Loans.GetLoan;

/// <summary>
/// Represents detailed information about a specific loan.
/// </summary>
public sealed record GetLoanResponse(
    /// <summary>
    /// The total amount of the loan.
    /// </summary>
    decimal Amount,
    /// <summary>
    /// The current balance remaining on the loan.
    /// </summary>
    decimal? CurrentBalance,
    /// <summary>
    /// The name of the loan applicant.
    /// </summary>
    string ApplicantName,
    /// <summary>
    /// The current status of the loan.
    /// </summary>
    LoanStatus Status,
    /// <summary>
    /// The amount that was already paid.
    /// </summary>
    decimal TotalPaidAmount,
    /// <summary>
    /// The date the loan was created.
    /// </summary>
    DateTime DateCreated,
    /// <summary>
    /// The date the loan was last updated, if any.
    /// </summary>
    DateTime? DateUpdated,
    /// <summary>
    /// The payments of the loan.
    /// </summary>
    IEnumerable<GetLoanPaymentResponse> Payments);

public sealed record GetLoanPaymentResponse(
    decimal Amount,
    DateTime DateCreated
);
