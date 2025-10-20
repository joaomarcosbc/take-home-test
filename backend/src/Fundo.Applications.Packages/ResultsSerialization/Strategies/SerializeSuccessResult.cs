using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.Applications.Packages.ResultsSerialization.Strategies;

public sealed class SerializeSuccessResult : ISerializeStrategy
{
    public bool ShouldSerialize(Result result) => result.IsSuccess;
    public bool ShouldSerialize<TResult>(Result<TResult> result) => result.IsSuccess;

    public IActionResult Serialize(Result result)
    {
        return new NoContentResult();
    }

    public IActionResult Serialize<TResult>(Result<TResult> result)
    {
        return new OkObjectResult(result.Value);
    }
}
