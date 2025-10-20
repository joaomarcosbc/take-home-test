using FluentResults;
using Fundo.Applications.Packages.ResultsSerialization.Strategies;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.Applications.Packages.ResultsSerialization.Serializer;

public sealed class Serializer
{
    private readonly IEnumerable<ISerializeStrategy> _strategies;

    public Serializer(IEnumerable<ISerializeStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IActionResult Serialize<T>(Result<T?> result) where T : class
    {
        var strategy = _strategies.First(s => s.ShouldSerialize(result));
        return strategy.Serialize(result);
    }

    public IActionResult Serialize(Result result)
    {
        var strategy = _strategies.First(s => s.ShouldSerialize(result));
        return strategy.Serialize(result);
    }
}
