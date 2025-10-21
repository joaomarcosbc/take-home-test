using Fundo.Applications.Application.UseCases.Loans.CreateLoan;
using Fundo.Applications.Application.UseCases.Loans.GetLoan;
using Fundo.Applications.Application.UseCases.Loans.CreateLoanPayment;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;
using Fundo.Applications.Application.Repositories;

namespace Fundo.Services.Tests.Integration;

public class LoanManagementControllerStub : ControllerBase
{
    private readonly ILoanRepository _loanRepo;
    private readonly ILoanPaymentRepository _paymentRepo;

    public LoanManagementControllerStub(ILoanRepository loanRepo, ILoanPaymentRepository paymentRepo)
    {
        _loanRepo = loanRepo;
        _paymentRepo = paymentRepo;
    }

    public async Task<IActionResult> Create(CreateLoanRequest request)
    {
        var loan = new Loan
        {
            Amount = request.Amount,
            CurrentBalance = (decimal)request.CurrentBalance,
            ApplicantName = request.ApplicantName,
            Status = LoanStatus.Active,
            DateCreated = DateTime.UtcNow
        };
        await _loanRepo.CreateAsync(loan);
        return NoContent();
    }

    public async Task<IActionResult> GetById(int id)
    {
        var loan = await _loanRepo.GetByIdAsync(id);
        if (loan == null) return NotFound();

        return Ok(new GetLoanResponse(
            Amount: loan.Amount,
            CurrentBalance: loan.CurrentBalance,
            ApplicantName: loan.ApplicantName,
            Status: loan.Status,
            TotalPaidAmount: loan.Payments.Sum(p => p.Amount),
            DateCreated: loan.DateCreated,
            DateUpdated: loan.DateModified,
            Payments: loan.Payments.Select(p => new GetLoanPaymentResponse(p.Amount, p.DateCreated))
        ));
    }

    public async Task<IActionResult> CreatePayment(int id, CreateLoanPaymentBody request)
    {
        var loan = await _loanRepo.GetByIdAsync(id);
        if (loan == null) return NotFound();

        loan.CurrentBalance -= request.Amount;
        loan.Status = loan.CurrentBalance == 0 ? LoanStatus.Paid : LoanStatus.Active;

        await _loanRepo.UpdateAsync(loan);

        await _paymentRepo.CreateAsync(new LoanPayment
        {
            LoanId = loan.Id,
            Amount = request.Amount,
            DateCreated = DateTime.UtcNow
        });

        return NoContent();
    }
}
