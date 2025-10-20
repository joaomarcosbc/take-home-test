using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fundo.Applications.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (response is Result result && result.IsFailed)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Request {RequestName} failed: {Error}", typeof(TRequest).Name, error.Message);
            }
        }

        return response;
    }
}
