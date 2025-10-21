using FluentResults;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using MediatR;

namespace Fundo.Applications.Application.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var error = new InternalServerError(ex.Message, ex);

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var tResultType = typeof(TResponse).GetGenericArguments()[0];
                var failMethod = typeof(Result)
                    .GetMethods()
                    .First(m => m.Name == nameof(Result.Fail) && m.IsGenericMethod)
                    .MakeGenericMethod(tResultType);

                return (TResponse)failMethod.Invoke(null, new object[] { error })!;
            }
            return (TResponse)(object)Result.Fail(error);
        }
    }
}
