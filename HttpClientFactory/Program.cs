using HttpClientFactory;
using HttpClientFactory.Components;
using HttpClientFactory.Handlers;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Polly;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<LoggingHandler>();
builder.Services.AddScoped<AuthHandler>();

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddHttpClientInstrumentation(options =>
    {
        options.EnrichWithHttpRequestMessage = (activity, message) =>
        {
            activity.SetTag("version", "1.0 beta");
        };
    })
    .AddConsoleExporter()
    .Build();

builder.Services.AddHttpClient<WeatherClient>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7060");
    })
    .AddHttpMessageHandler<AuthHandler>()
    .AddHttpMessageHandler<LoggingHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
