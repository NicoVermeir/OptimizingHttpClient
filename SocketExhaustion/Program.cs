using System.Net.Sockets;

#region SocketExhaustionDemo

Console.WriteLine("Starting socket exhaustion demo...");

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
            throw new Exception();
        }
    });

await Task.WhenAll(tasks);
#endregion


#region SP

//using var client = new HttpClient();
//var response = await client.GetStringAsync("https://localhost:7060/weatherforecast");
//Console.WriteLine(response);

#endregion