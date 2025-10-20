using FluentResults;
using Fundo.Applications.Domain.Enums;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.CreateLoan;

/// <summary>
/// Represents a request to create a new loan.
/// </summary>
public sealed record CreateLoanRequest(
    /// <summary>
    /// The total amount of the loan.
    /// </summary>
    decimal Amount,
    /// <summary>
    /// The current balance of the loan (optional).
    /// </summary>
    decimal? CurrentBalance,
    /// <summary>
    /// The name of the loan applicant.
    /// </summary>
    string ApplicantName
) : IRequest<Result>;