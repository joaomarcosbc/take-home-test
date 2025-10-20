using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Infrastructure.DatabaseContext;
using Fundo.Applications.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Applications.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FundoDatabaseContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("FundoDatabaseConnectionString"));
        });

        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services) 
    {
        services
            .AddScoped<ILoanRepository, LoanRepository>()
            .AddScoped<ILoanPaymentRepository, LoanPaymentRepository>();

        return services;
    }
}
