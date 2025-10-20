using FluentResults;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.GetLoan;

/// <summary>
/// Represents a request to retrieve a specific loan by its ID.
/// </summary>
public sealed record GetLoanRequest(
    /// <summary>
    /// The unique identifier of the loan.
    /// </summary>
    int Id
) : IRequest<Result<GetLoanResponse>>;