using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpClientFactory;

public class WeatherClient(HttpClient client)
{
    public async Task<WeatherForecast[]> GetWeatherAsync()
    {
        HttpResponseMessage response = await client.GetAsync("/weatherforecasts");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

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