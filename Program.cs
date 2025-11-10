Console.WriteLine("Starting socket exhaustion demo...\n");

var tasks = Enumerable.Range(0, 20000)
    .Select(async i =>
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7060/weatherforecast");
            Console.WriteLine($"{i}: {response.StatusCode}");
        }
        catch (HttpRequestException ex) when (ex.InnerException is SocketException)
        {
            Console.WriteLine($"{i}: SOCKET EXHAUSTED - {ex.InnerException.Message}");
        }
    });

await Task.WhenAll(tasks);