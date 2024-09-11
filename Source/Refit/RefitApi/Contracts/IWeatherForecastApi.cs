using Refit;
using RefitApi.Models;

namespace RefitApi.Contracts;

public interface  IWeatherForecastApi
{
    [Get("/weatherforecast")]
    Task<List<WeatherForecast>> GetWeathersAsync();
    
    [Get("/weathermessage")]
    Task<HttpResponseMessage> GetWeatherMessageAsync();
    
    [Get("/weatherapi")]
    Task<ApiResponse<WeatherForecast>> GetWeatherApiAsync();
}