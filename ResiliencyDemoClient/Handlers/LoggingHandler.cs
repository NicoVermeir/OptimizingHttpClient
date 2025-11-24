namespace ResiliencyDemoClient.Handlers;

public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"==== HANDLER: Logging request to: {request.RequestUri}");

        var response = await base.SendAsync(request, cancellationToken);
        Console.WriteLine($"=== Response received: HTTP {response.StatusCode}");

        return response;
    }
}
