using FluentResults;
using Fundo.Applications.Application.MappingProfiles;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.GetLoan;

/// <summary>
/// Retrieves a loan by its ID.
/// </summary>
/// <param name="request">The <see cref="GetLoanRequest"/> containing the loan ID.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>
/// <see cref="Result{GetLoanResponse}"/> with the loan details or 404 if not found.
/// </returns>
internal sealed class GetLoanHandler : IRequestHandler<GetLoanRequest, Result<GetLoanResponse>>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoanHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<Result<GetLoanResponse>> Handle(GetLoanRequest request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.Id);

        if (loan == null)
            return new NotFoundError("No loan found.");

        return Result.Ok(loan.ToGetLoanResponse());
    }
}
