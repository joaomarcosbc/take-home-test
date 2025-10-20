using FluentResults;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fundo.Applications.Packages.ResultsSerialization.Strategies;

internal sealed class SerializeInternalServerErrorResult : ISerializeStrategy
{
    public bool ShouldSerialize(Result result) => result.Errors.Any(e => e is InternalServerError);

    public bool ShouldSerialize<TResult>(Result<TResult> result) => result.Errors.Any(e => e is InternalServerError);

    public IActionResult Serialize(Result result)
    {
        var details = new ProblemDetails()
        {
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = (int)HttpStatusCode.InternalServerError
        };

        return new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    public IActionResult Serialize<TResult>(Result<TResult> result)
    {
        var details = new ProblemDetails()
        {
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = (int)HttpStatusCode.InternalServerError
        };

        return new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
