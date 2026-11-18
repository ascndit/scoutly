namespace scoutly.Startup;

public static partial class Startup
{
    private const string FrontendPolicyName = "FrontendPolicy";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicyName, policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static WebApplication UseCorsConfiguration(this WebApplication app)
    {
        app.UseCors(FrontendPolicyName);
        return app;
    }
}