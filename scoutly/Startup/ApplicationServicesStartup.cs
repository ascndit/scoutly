using scoutly.Abstraction;
using scoutly.Extensions;

namespace scoutly.Startup;

public static class ApplicationServicesStartup
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddHandlers();

        return services;
    }
}