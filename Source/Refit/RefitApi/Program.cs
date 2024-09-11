using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using RefitApi.Contracts;
using RefitApi.Models;

namespace RefitApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddRefitClient<IWeatherForecastApi>(new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                })
            })
            .ConfigureHttpClient(config =>
            {
                config.Timeout = new TimeSpan(0,0,1,0);
                config.BaseAddress = new Uri("https://localhost:44381");
                
                
            });
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapGet("/weatherforecast", async (IWeatherForecastApi api) =>
        {
            var list = await api.GetWeathersAsync();
            return new ActionResult<List<WeatherForecast>>(list);
        });

        app.MapControllers();
        app.UseHttpsRedirection();

        
        
        app.Run();
    }
}