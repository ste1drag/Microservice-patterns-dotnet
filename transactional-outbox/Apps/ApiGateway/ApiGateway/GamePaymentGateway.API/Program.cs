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

    var gameTicketTask=  gameClient.GetFromJsonAsync<object>(
        $"api/Game/get-ticket-info/{gameTicketId}", ct);
    var transactionTask =  paymentClient.GetFromJsonAsync<object>(
        $"api/Payment/get-transaction-info/game/{gameTicketId}/transaction/{transactionId}", ct);

    await Task.WhenAll(gameTicketTask, transactionTask);

    return Results.Ok(new
    {
        gameTicketId,
        transactionId,
        gameTicketInfo = gameTicketTask.Result,
        transactionInfo = transactionTask.Result
    });
});

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
