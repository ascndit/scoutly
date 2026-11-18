namespace scoutly.Startup;

public static partial class Startup
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDatabaseServices(builder.Configuration);
        builder.Services.AddServices();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddCorsConfiguration();
        
        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
        }
        else
        {
            app.UseHttpsRedirection();
        }
        
        app.UseCorsConfiguration();
        app.UseAuthorization();
        app.MapControllers();
        
        return app;
    }
}