using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var gameApiBase = builder.Configuration["ReverseProxy:Clusters:game-cluster:Destinations:game-api:Address"]
    ?? throw new InvalidOperationException("game-cluster destination not configured");
var paymentApiBase = builder.Configuration["ReverseProxy:Clusters:payment-cluster:Destinations:payment-api:Address"]
    ?? throw new InvalidOperationException("payment-cluster destination not configured");

builder.Services.AddHttpClient("game", c => c.BaseAddress = new Uri(gameApiBase));
builder.Services.AddHttpClient("payment", c => c.BaseAddress = new Uri(paymentApiBase));

var app = builder.Build();

app.MapGet("/GetGameTransaction/game/{gameTicketId:guid}/transaction/{transactionId:guid}", async (
    Guid gameTicketId,
    Guid transactionId,
    [FromServices] IHttpClientFactory httpClientFactory,
    CancellationToken ct) =>
{
    var gameClient = httpClientFactory.CreateClient("game");
    var paymentClient = httpClientFactory.CreateClient("payment");

    var gameTicketTask = GetJsonOrNullAsync(
        gameClient,
        $"api/Game/get-ticket-info/{gameTicketId}",
        ct);
    var transactionTask = GetJsonOrNullAsync(
        paymentClient,
        $"api/Payment/get-transaction-info/game/{gameTicketId}/transaction/{transactionId}",
        ct);

    await Task.WhenAll(gameTicketTask, transactionTask);

    var gameTicketInfo = await gameTicketTask;
    var transactionInfo = await transactionTask;

    if (gameTicketInfo is null && transactionInfo is null)
    {
        return Results.NotFound(new
        {
            gameTicketId,
            transactionId,
            message = "Neither game ticket nor transaction was found."
        });
    }

    return Results.Ok(new
    {
        gameTicketId,
        transactionId,
        gameTicketInfo,
        transactionInfo
    });
});

static async Task<object?> GetJsonOrNullAsync(HttpClient client, string requestUri, CancellationToken ct)
{
    using var response = await client.GetAsync(requestUri, ct);

    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        return null;
    }

    response.EnsureSuccessStatusCode();

    if (response.StatusCode == System.Net.HttpStatusCode.NoContent
        || response.Content.Headers.ContentLength == 0)
    {
        return null;
    }

    var content = await response.Content.ReadAsStringAsync(ct);
    if (string.IsNullOrWhiteSpace(content))
    {
        return null;
    }

    return System.Text.Json.JsonSerializer.Deserialize<object>(content);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapReverseProxy();

app.Run();
