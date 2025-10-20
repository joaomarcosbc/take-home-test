using FluentResults;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.CreateLoanPayment;

/// <summary>
/// Represents a request to create a new loan payment.
/// </summary>
public sealed record CreateLoanPaymentRequest(
    /// <summary>
    /// The unique identifier of the loan to which the payment applies.
    /// </summary>
    int LoanId,
    /// <summary>
    /// The amount of the payment.
    /// </summary>
    decimal Amount
) : IRequest<Result>;

public sealed record CreateLoanPaymentBody(
    /// <summary>
    /// The amount of the payment.
    /// </summary>
    decimal Amount
);
