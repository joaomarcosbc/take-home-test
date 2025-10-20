using FluentResults;

namespace Fundo.Applications.Packages.ResultsSerialization.Errors;

public class InternalServerError : Error
{
    public InternalServerError(string? message, Exception? exception) : base(message)
    {
        if (exception != null)
            CausedBy(exception);
    }
}
