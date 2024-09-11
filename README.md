# What is Refit?
Refit is a type-safe REST library for .NET. You can allow to define Api as an interface. This approach make your API calls readable and maintanable.

Benefits of Refit includes these part:

1. Handles serialization and deserialization objects.
2. Handle exceptions in comile time.
3. Support various HTTP methods.
4. Handle headers and specific types in a straightforward way.

## Setting up and Using refit

Consume that we have a api with 3 endpoint. 
At first we want to use minimal api in my app and call the another api.

At first you should add package that requided:

``` shell
Install-Package Refit
Install-Package Refit.HttpClientFactory
```

Now can create class according our need :

``` c#
public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
```

``` c#
public interface  IWeatherForecastApi
{
    [Get("/weatherforecast")]
    Task<List<WeatherForecast>> GetWeathersAsync();
}
```

Now we setup Refit in program.cs.

``` c#
        builder.Services.AddRefitClient<IWeatherForecastApi>()
            .ConfigureHttpClient(config =>
            {
                config.Timeout = new TimeSpan(0,0,1,0);
                config.BaseAddress = new Uri("https://localhost:44381");
                
                
            });
```


now we have to implement Minimal API endpoints like this :

``` c#
        app.MapGet("/weatherforecast", async (IWeatherForecastApi api) =>
        {
            var list = await api.GetWeathersAsync();
            return new ActionResult<List<WeatherForecast>>(list);
        });
```


Also you can `Refit.Newtonsoft.Json` package and config json serialize and deserialize objcts:

``` c#
        builder.Services.AddRefitClient<IWeatherForecastApi>(new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                })
            })
```


Now we want to call the api's in controller.
If you remember that serialization and deserialization is one of the facilities of Refit, but you can get data as HttpResponseMssage and ApiResponse\<T\>.

So we defined interface like this :
``` c#
public interface  IWeatherForecastApi
{
    [Get("/weatherforecast")]
    Task<List<WeatherForecast>> GetWeathersAsync();
    
    [Get("/weathermessage")]
    Task<HttpResponseMessage> GetWeatherMessageAsync();
    
    [Get("/weatherapi")]
    Task<ApiResponse<WeatherForecast>> GetWeatherApiAsync();
}
```

Corresponding interface we define controller like this :

```c#
[ApiController]
[Route("/wrapper")]
public class WrapperController : ControllerBase
{
    [Route("/wehathers")]
    [HttpGet]
    public async Task<IActionResult> GetWeatherAsync(IWeatherForecastApi api)
    {
        var result = await api.GetWeathersAsync();

        return Ok(result);
    }
    
    [Route("/wehathers/httpmessage")]
    [HttpGet]
    public async Task<IActionResult> GetHttpMessageAsync(IWeatherForecastApi api)
    {
        var result = await api.GetWeatherMessageAsync();

        if (result.IsSuccessStatusCode)
        {
            var data = await result.Content.ReadAsStringAsync();
            return Ok(data);
        }

        return BadRequest();
    }
    [Route("/wehathers/wehatherapi")]
    [HttpGet]
    public async Task<IActionResult> GetMessageApiAsync(IWeatherForecastApi api)
    {
        var result = await api.GetWeatherApiAsync();
        if (result.IsSuccessStatusCode)
        {
            return Ok(result.Content);            
        }

        return BadRequest();
    }
    
}
```

