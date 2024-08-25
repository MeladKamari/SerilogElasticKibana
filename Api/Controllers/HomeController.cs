using Application;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HomeController(ILogger<HomeController> logger,IWeatherService weatherService) : ControllerBase
{
    [HttpGet]
    public IEnumerable<WeatherForecastDto> GetWeather() => weatherService.GetWeather();
    [HttpGet]
    public IActionResult LogSensor()
    {
        weatherService.LogSensor();
        return Ok();
    }
}