using Microsoft.OpenApi.Models;

namespace scoutly.Startup;

public static partial class Startup
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Scoutly API (Vertical Slice Architecture)",
                Version = "v1",
                Description = "REST API комплекса скаутинга, матчинга специалистов и управления вакансиями"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scoutly API v1");
            c.RoutePrefix = "swagger";
        });

        return app;
    }
}