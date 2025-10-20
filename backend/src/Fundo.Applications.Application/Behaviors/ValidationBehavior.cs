using FluentResults;
using FluentValidation;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Fundo.Applications.Packages.ResultsSerialization.Serializer;
using MediatR;

namespace Fundo.Applications.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var error = new BadRequestError(
                "Validation failed for one or more fields.",
                failures.Select(f => f.ErrorMessage).ToList());

            var result = Result.Fail(error);

            return (TResponse)(object)result;
        }

        return await next();
    }
}
