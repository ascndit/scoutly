using scoutly.Abstraction;
using scoutly.Extensions;

namespace scoutly.Startup;

public static partial class Startup
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddHandlers();

        return services;
    }
}