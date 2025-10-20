using FluentResults;

namespace Fundo.Applications.Packages.ResultsSerialization.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
    }
}