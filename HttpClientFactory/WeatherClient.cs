using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpClientFactory;

public class WeatherClient(HttpClient client)
{
    public async Task<WeatherForecast[]> GetWeatherAsync()
    {
        string json = await client.GetStringAsync("/weatherforecasts");
        return JsonSerializer.Deserialize<WeatherForecast[]>(json);
    }
}

public class WeatherForecast
{
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    [JsonPropertyName("temperatureC")]
    public int TemperatureC { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
}