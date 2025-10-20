using FluentResults;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Fundo.Applications.Application.MappingProfiles;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.GetAllLoans;

/// <summary>
/// Retrieves a paginated list of loans.
/// </summary>
/// <param name="request">Pagination parameters in <see cref="GetAllLoansRequest"/>.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns><see cref="Result{T}"/> containing the loan list or 404 if none found.</returns>
internal sealed class GetAllLoansHandler : IRequestHandler<GetAllLoansRequest, Result<List<GetAllLoansResponse>>>
{
    private readonly ILoanRepository _loanRepository;
    public GetAllLoansHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }
    public async Task<Result<List<GetAllLoansResponse>>> Handle(GetAllLoansRequest request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetAllAsync(
            request.PageNumber,
            request.PageSize);

        if (!loans.Any())
            return new NotFoundError($"No loans found on page {request.PageNumber}.");

        return loans
            .ToGetAllLoansResponse()
            .ToList();
    }
}
