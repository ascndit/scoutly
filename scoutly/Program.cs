using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Extensions;

var builder = WebApplication.CreateBuilder(args);
// User Secrets || appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext");

// ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.UseSnakeCaseNamingConvention();
});

//TODO Создать папку startup и разделить логически регистрацию сервисов
// Controllers || Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Scoutly API",
        Version = "v1",
        Description = "API ������� ��������� ������������"
    });
});

builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddHandlers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scoutly API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
