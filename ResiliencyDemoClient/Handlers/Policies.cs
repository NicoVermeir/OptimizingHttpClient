using Polly;
using Polly.Extensions.Http;

namespace ResiliencyDemoClient.Handlers;

public static class Policies
{
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(this PolicyBuilder<HttpResponseMessage> builder)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(5),
                onBreak: (outcome, timespan) =>
                {
                    Console.WriteLine($"==> Circuit breaker opened! Will stay open for {timespan.TotalSeconds} seconds.");
                },
                onReset: () =>
                {
                    Console.WriteLine("==> Circuit breaker reset! Circuit is now closed.");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("==> Circuit breaker is half-open. Testing if service recovered...");
                });
    }
}