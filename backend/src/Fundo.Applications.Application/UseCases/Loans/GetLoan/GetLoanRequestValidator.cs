using FluentValidation;

namespace Fundo.Applications.Application.UseCases.Loans.GetLoan;

public sealed class GetLoanRequestValidator : AbstractValidator<GetLoanRequest>
{
    public GetLoanRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0.");
    }
}