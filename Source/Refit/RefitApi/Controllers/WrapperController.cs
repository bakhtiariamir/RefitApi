using Microsoft.AspNetCore.Mvc;
using RefitApi.Contracts;
using RefitApi.Models;

namespace RefitApi.Controllers;

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