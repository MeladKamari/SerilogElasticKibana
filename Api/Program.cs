using Api.Extensions;
using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Host.AddSerilog();
var services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddScoped<IWeatherService, WeatherService>();
services.AddScoped<IPersonService, PersonService>();
services.AddControllers();
services.AddDbContext<ApplicationContext>(q => q.UseSqlite("Data Source=applicationdatabase.db")
#if DEBUG
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddSerilog()))
#endif
);
var app = builder.Build();
app.Use(async (context, next) =>
{
  var userId = context.Request.Headers["UserId"]!;
    using (LogContext.PushProperty("UserId", userId))
    {
        await next.Invoke();
    }
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/weatherforecast", (ILogger<Program> logger, IWeatherService weatherService) =>
    {
        logger.LogInformation("Minimal Api");
        return weatherService.GetWeather();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
app.UseSerilogRequestLogging();
app.MapControllers();
app.Run();