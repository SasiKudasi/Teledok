using Microsoft.Extensions.DependencyInjection;
using Teledok.Application.Services;

namespace Teledok.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();
        return services;
    }
}
