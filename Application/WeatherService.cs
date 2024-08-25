using Application.Dtos;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application;

public class WeatherService(ILogger<WeatherService> logger) : IWeatherService
{
    public void LogSensor()
    {
        var fruit = new Dictionary<string, int> { { "Apple", 1 }, { "Pear", 5 } };
        logger.LogInformation("In my bowl I have {Fruit}", fruit);
        var sensorInput = new { Latitude = 25, Longitude = 134 };
        logger.LogInformation("Processing {@SensorInput}, Time : {time}", sensorInput,
            TimeProvider.System.GetUtcNow().DateTime);
    }

    public IEnumerable<WeatherForecastDto> GetWeather()
    {
        using (LogContext.PushProperty("ServiceName", nameof(WeatherService)))
        using (LogContext.PushProperty("MethodName", nameof(GetWeather)))
        {
            logger.LogInformation("Section 01");
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            logger.LogInformation("Section 02");
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecastDto
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                .ToArray();
            logger.LogInformation("Section 03");
            return forecast;
        }
    }
}