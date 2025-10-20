using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.Applications.Packages.ResultsSerialization.Strategies;

/// <summary>
/// Defines a strategy for serializing <see cref="FluentResults.Result"/> and <see cref="FluentResults.Result{TResult}"/> 
/// objects into <see cref="IResult"/> responses, typically for API error handling.
/// </summary>
public interface ISerializeStrategy
{
    /// <summary>
    /// Determines whether this strategy can serialize the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <returns><c>true</c> if this strategy can serialize the result; otherwise, <c>false</c>.</returns>
    bool ShouldSerialize(Result result);

    /// <summary>
    /// Determines whether this strategy can serialize the specified <see cref="Result{TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="result">The result to evaluate.</param>
    /// <returns><c>true</c> if this strategy can serialize the result; otherwise, <c>false</c>.</returns>
    bool ShouldSerialize<TResult>(Result<TResult> result);

    /// <summary>
    /// Serializes the specified <see cref="Result"/> into an <see cref="IResult"/>.
    /// </summary>
    /// <param name="result">The result to serialize.</param>
    /// <returns>An <see cref="IActionResult"/> representing the serialized result, typically as an HTTP response.</returns>
    IActionResult Serialize(Result result);

    /// <summary>
    /// Serializes the specified <see cref="Result{TResult}"/> into an <see cref="IResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="result">The result to serialize.</param>
    /// <returns>An <see cref="IActionResult"/> representing the serialized result, typically as an HTTP response.</returns>
    IActionResult Serialize<TResult>(Result<TResult> result);
}
