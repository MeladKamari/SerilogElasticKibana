using Application.Dtos;

namespace Application;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetWeather();
}