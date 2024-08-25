using Application.Dtos;

namespace Application;

public interface IWeatherService
{
    IEnumerable<WeatherForecastDto> GetWeather();
    void LogSensor();
}