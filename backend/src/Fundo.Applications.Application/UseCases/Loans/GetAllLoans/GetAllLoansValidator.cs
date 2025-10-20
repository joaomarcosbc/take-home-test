using FluentValidation;

namespace Fundo.Applications.Application.UseCases.Loans.GetAllLoans;

internal sealed class GetAllLoansValidator : AbstractValidator<GetAllLoansRequest>
{
    private const int MaxPageSize = 100;

    public GetAllLoansValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("{PropertyName} must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("{PropertyName} must be greater than or equal to 1.")
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage("{PropertyName} cannot exceed " + MaxPageSize + ".");
    }
}
