using FluentResults;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fundo.Applications.Packages.ResultsSerialization.Strategies;

internal sealed class SerializeNotFoundServerErrorResult : ISerializeStrategy
{
    public bool ShouldSerialize(Result result) => result.Errors.Any(e => e is NotFoundError);

    public bool ShouldSerialize<TResult>(Result<TResult> result) => result.Errors.Any(e => e is NotFoundError);

    public IActionResult Serialize(Result result)
    {
        var details = new ProblemDetails()
        {
            Title = "Not Found Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            Detail = result.Errors[0].Message,
            Status = (int)HttpStatusCode.NotFound
        };

        return new NotFoundObjectResult(details);
    }

    public IActionResult Serialize<TResult>(Result<TResult> result)
    {
        var details = new ProblemDetails()
        {
            Title = "Not Found Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            Detail = result.Errors[0].Message,
            Status = (int)HttpStatusCode.NotFound
        };

        return new NotFoundObjectResult(details);
    }
}