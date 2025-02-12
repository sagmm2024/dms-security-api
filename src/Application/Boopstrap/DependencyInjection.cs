using Andreani.Arq.Core.Pipeline.Extension;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SecurityApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAndreaniPipeline(Verbose: true);

        return services;
    }
}
