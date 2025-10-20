using FluentResults;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fundo.Applications.Packages.ResultsSerialization.Strategies;

internal sealed class SerializeBadRequestErrorResult : ISerializeStrategy
{
    public bool ShouldSerialize(Result result) => result.Errors.Any(e => e is BadRequestError);

    public bool ShouldSerialize<TResult>(Result<TResult> result) => result.Errors.Any(e => e is BadRequestError);

    public IActionResult Serialize(Result result)
    {
        var badRequest = result.Errors.OfType<BadRequestError>().First();

        var details = new ProblemDetails()
        {
            Title = "Bad Request Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Detail = badRequest.Message,
            Status = (int)HttpStatusCode.BadRequest
        };

        if (badRequest.ValidationMessages?.Count >= 1)
            details.Extensions["errors"] = badRequest.ValidationMessages;

        return new BadRequestObjectResult(details);
    }

    public IActionResult Serialize<TResult>(Result<TResult> result)
    {
        var badRequest = result.Errors.OfType<BadRequestError>().First();

        var details = new ProblemDetails()
        {
            Title = "Bad Request Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Detail = badRequest.Message,
            Status = (int)HttpStatusCode.BadRequest
        };

        if (badRequest.ValidationMessages?.Count >= 1)
            details.Extensions["errors"] = badRequest.ValidationMessages;

        return new BadRequestObjectResult(details);
    }
}
