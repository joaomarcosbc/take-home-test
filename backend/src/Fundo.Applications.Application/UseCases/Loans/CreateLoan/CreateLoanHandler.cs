using FluentResults;
using Fundo.Applications.Application.MappingProfiles;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Domain.Enums;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.CreateLoan;

/// <summary>
/// Creates a new loan.
/// </summary>
/// <param name="request">Loan data in <see cref="CreateLoanRequest"/>.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns><see cref="Result"/> indicating success or failure.</returns>
public sealed class CreateLoanHandler : IRequestHandler<CreateLoanRequest, Result>
{
    private readonly ILoanRepository _loanRepository;
    public CreateLoanHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }
    public async Task<Result> Handle(CreateLoanRequest request, CancellationToken cancellationToken)
    {
        var loan = request.ToEntity();

        loan.Status = loan.CurrentBalance == 0
         ? LoanStatus.Paid
         : LoanStatus.Active;

        await _loanRepository.CreateAsync(loan);

        return Result.Ok();
    }
}
