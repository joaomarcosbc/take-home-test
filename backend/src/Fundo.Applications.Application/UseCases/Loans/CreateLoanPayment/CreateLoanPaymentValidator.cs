using FluentValidation;

namespace Fundo.Applications.Application.UseCases.Loans.CreateLoanPayment;

public sealed class CreateLoanPaymentValidator : AbstractValidator<CreateLoanPaymentRequest>
{
    public CreateLoanPaymentValidator()
    {
        RuleFor(x => x.LoanId)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0.");
    }
}
