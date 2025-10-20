using Fundo.Applications.Application.UseCases.Loans.CreateLoan;
using Fundo.Applications.Application.UseCases.Loans.GetAllLoans;
using Fundo.Applications.Application.UseCases.Loans.GetLoan;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;

namespace Fundo.Applications.Application.MappingProfiles;

public static class LoanMappings
{
    public static Loan ToEntity(this CreateLoanRequest request)
    {
        return new Loan
        {
            Amount = request.Amount,
            CurrentBalance = request.CurrentBalance ?? 0m,
            ApplicantName = request.ApplicantName
        };
    }

    public static GetAllLoansResponse ToGetAllLoansResponse(this Loan model)
    {
        return new GetAllLoansResponse(
            model.Id,
            model.Amount,
            model.CurrentBalance,
            model.ApplicantName,
            model.Status);
    }

    public static IEnumerable<GetAllLoansResponse> ToGetAllLoansResponse(this IEnumerable<Loan> models)
    {
        return models.Select(m => m.ToGetAllLoansResponse());
    }

    public static GetLoanResponse ToGetLoanResponse(this Loan loan)
    {
        var payments = loan.Payments?.Select(p => new GetLoanPaymentResponse(
            p.Amount,
            p.DateCreated
        )) ?? Enumerable.Empty<GetLoanPaymentResponse>();

        return new GetLoanResponse(
            loan.Amount,
            loan.CurrentBalance,
            loan.ApplicantName,
            loan.Status,
            loan.Amount - loan.CurrentBalance,
            loan.DateCreated,
            loan.DateModified,
            payments
        );
    }
}
