using System.Net;

namespace HttpClientFactory.Handlers;

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

public class AuthHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("==== HANDLER: Adding auth header");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "MyToken");

        var response = await base.SendAsync(request, cancellationToken);

        Console.WriteLine("=== Received authenticated response");

        return response;
    }
}