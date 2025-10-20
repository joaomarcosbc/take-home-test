using Fundo.Applications.Packages.ResultsSerialization.Serializer;
using Fundo.Applications.Packages.ResultsSerialization.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Fundo.Applications.Packages;

public static class PackagesServiceRegistration
{
    public static IServiceCollection AddPackagesServices(this IServiceCollection services)
    {
        services.AddScoped<Serializer>();
        services.AddScoped<ISerializeStrategy, SerializeBadRequestErrorResult>();
        services.AddScoped<ISerializeStrategy, SerializeNotFoundServerErrorResult>();
        services.AddScoped<ISerializeStrategy, SerializeInternalServerErrorResult>();
        services.AddScoped<ISerializeStrategy, SerializeSuccessResult>();
        return services;
    }
}
