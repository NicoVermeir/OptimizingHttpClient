using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async () =>
{
    await Task.Delay(2000);
    return new WeatherForecast
    (
        DateOnly.FromDateTime(DateTime.Now),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]
    );
});

app.MapGet("/weatherforecasts", async () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray();
    return forecast;
});

var requestCount = 0;
var random = new Random();
app.MapGet("/api/flakyapi/weather", (int failureRate = 50) =>
{
    var currentRequest = Interlocked.Increment(ref requestCount);

    // Simulate random failures based on failure rate
    if (random.Next(100) < failureRate)
    {
        return Results.Json(
            new { error = "Service temporarily unavailable", attempt = currentRequest },
            statusCode: 503);
    }

    return Results.Ok(new
    {
        temperature = random.Next(-10, 35),
        summary = "Sunny",
        attempt = currentRequest,
        timestamp = DateTime.UtcNow
    });
});

app.MapGet("/api/flakyapi/always-fail", (int attempts = 0) =>
{
    var currentRequest = Interlocked.Increment(ref requestCount);

    // Fail for the first N attempts, then succeed
    if (currentRequest <= attempts)
    {
        return Results.Json(
            new
            {
                error = "Service unavailable",
                attempt = currentRequest,
                message = $"Will succeed after {attempts} attempts"
            },
            statusCode: 503);
    }

    // Reset counter and succeed
    Interlocked.Exchange(ref requestCount, 0);
    return Results.Ok(new
    {
        temperature = 22,
        summary = "Success after retries!",
        attempt = currentRequest,
        timestamp = DateTime.UtcNow
    });
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);
