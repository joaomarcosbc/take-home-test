using FluentResults;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using MediatR;

namespace Fundo.Applications.Application.UseCases.Loans.CreateLoanPayment;

/// <summary>
/// Records a payment for a loan.
/// </summary>
/// <param name="request">Payment details in <see cref="CreateLoanPaymentRequest"/>.</param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>
/// <see cref="Result"/>:
/// 204 on success, 400 if amount is invalid, 404 if loan not found.
/// </returns>
internal sealed class CreateLoanPaymentHandler : IRequestHandler<CreateLoanPaymentRequest, Result>
{
    private readonly ILoanRepository _loanRepository;
    private readonly ILoanPaymentRepository _loanPaymentRepository;

    public CreateLoanPaymentHandler(
        ILoanRepository loanRepository,
        ILoanPaymentRepository loanPaymentRepository)
    {
        _loanRepository = loanRepository;
        _loanPaymentRepository = loanPaymentRepository;
    }

    public async Task<Result> Handle(CreateLoanPaymentRequest request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId);

        if (loan is null)
            return new NotFoundError("No loan found.");

        if (loan.Status is LoanStatus.Paid)
            return new BadRequestError("This loan was already paid.");

        if (request.Amount > (loan.Amount - (loan.Amount - loan.CurrentBalance)))
            return new BadRequestError("A payment cannot exceed the total loan amount.");

        loan.CurrentBalance -= request.Amount;

        loan.Status = loan.CurrentBalance == 0
         ? LoanStatus.Paid
         : LoanStatus.Active;

        await _loanRepository.UpdateAsync(loan);

        var payment = new LoanPayment
        {
            LoanId = loan.Id,
            Amount = request.Amount,
            DateCreated = DateTime.UtcNow
        };

        await _loanPaymentRepository.CreateAsync(payment);
        return Result.Ok();
    }
}
