using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Korfinae.Extensions.Logging.RemoteConsole;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .CreateLogger();
        
        builder.Host.UseSerilog();
        
        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
        });
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        //app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.UseCors(options => options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                .ToArray();
            return forecast;
        });
        
        
        app.MapPost("/logwrite",
        ([FromBody] LogEvent[] events,
            ILogger<Program> logger,
            HttpContext httpContext) =>
        {
            var nbrOfEvents = events.Length;
            var apiKey = httpContext.Request.Headers["X-Api-Key"].FirstOrDefault();
            //var events = await httpContext.Request.ReadFromJsonAsync<LogEvent>();
            
            //var body = new StreamReader(httpContext.Request.Body);
            //string postData = await body.ReadToEndAsync();
            
            //Console.WriteLine(nbrOfEvents);
            //Console.WriteLine(events);
            
            logger.LogInformation(
                "Received batch of {count} log events from {sender}",
                nbrOfEvents,
                apiKey);

            foreach (var logEvent in events)
            {
                logger.LogInformation("Message: {Message}", logEvent.RenderedMessage);
            }
            
            
            //Dictionary<string, dynamic> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(postData) ?? new Dictionary<string, dynamic>();
            // here after you can play as you like :)
            //return await Task.FromResult<string>(postData);

            return Results.Ok();

        });

        app.Run();
    }
}



public class LogEvent
{
    public DateTime Timestamp { get; set; }

    public string? Level { get; set; }

    public string? RenderedMessage { get; set; }
}