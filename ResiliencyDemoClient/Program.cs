using Polly;
using Polly.Extensions.Http;
using ResiliencyDemoClient.Components;
using ResiliencyDemoClient.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<LoggingHandler>();

// Create the retry policy we want
var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
    .GetCircuitBreakerPolicy();

builder.Services.AddHttpClient("weatherApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7060");
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<LoggingHandler>()
.AddPolicyHandler(retryPolicy);


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
