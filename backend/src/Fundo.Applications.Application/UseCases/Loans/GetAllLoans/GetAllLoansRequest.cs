using FluentResults;
using Fundo.Applications.Application.Common;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.GetAllLoans;

/// <summary>
/// Represents a request to retrieve a paginated list of loans.
/// </summary>
public record class GetAllLoansRequest(
    /// <summary>
    /// The page number to retrieve (default is 1).
    /// </summary>
    int PageNumber = 1,
    /// <summary>
    /// The number of items per page (default is 10).
    /// </summary>
    int PageSize = 10
) : IRequest<Result<PagedResult<GetAllLoansResponse>>>;
