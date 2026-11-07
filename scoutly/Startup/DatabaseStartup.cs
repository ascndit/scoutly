using Microsoft.EntityFrameworkCore;
using scoutly.Data.Context;

namespace scoutly.Startup;

public static class DatabaseStartup
{
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDbContext");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}