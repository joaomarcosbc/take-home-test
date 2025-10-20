using FluentResults;

namespace Fundo.Applications.Packages.ResultsSerialization.Errors;

public class BadRequestError : Error
{
    public IReadOnlyList<string>? ValidationMessages { get; }

    public BadRequestError(string message, IEnumerable<string>? validationMessages = null)
        : base(message)
    {
        ValidationMessages = validationMessages?.ToList();
    }
}