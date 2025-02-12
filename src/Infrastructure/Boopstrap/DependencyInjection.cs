using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace SecurityApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddCQRS<ApplicationDbContext>(configuration);

        //services.AddScoped<ApplicationDbContext>();

        return services;
    }
}
