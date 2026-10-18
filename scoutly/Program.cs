using Microsoft.EntityFrameworkCore;
using scoutly.Context;

var builder = WebApplication.CreateBuilder(args);
// User Secrets || appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.UseSnakeCaseNamingConvention();
});

// Controllers || Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Scoutly API",
        Version = "v1",
        Description = "API системы скаутинга специалистов"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scoutly API v1");
        c.RoutePrefix = "swagger"; //Swagger UI доступен по адресу /swagger
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
