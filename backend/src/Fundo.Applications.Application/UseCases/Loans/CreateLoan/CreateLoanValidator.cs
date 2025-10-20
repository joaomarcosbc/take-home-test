using FluentValidation;

namespace Fundo.Applications.Application.UseCases.Loans.CreateLoan;

public sealed class CreateLoanValidator : AbstractValidator<CreateLoanRequest>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than zero.");

        RuleFor(x => x.CurrentBalance)
                .GreaterThanOrEqualTo(0)
                .When(x => x.CurrentBalance.HasValue)
                .WithMessage("{PropertyName} cannot be negative.")
                .LessThanOrEqualTo(x => x.Amount)
                .When(x => x.CurrentBalance.HasValue)
                .WithMessage("{PropertyName} cannot exceed the total loan amount.");

        RuleFor(x => x.ApplicantName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(100)
            .WithMessage("{PropertyName} must not exceed 100 characters.");
    }
}
