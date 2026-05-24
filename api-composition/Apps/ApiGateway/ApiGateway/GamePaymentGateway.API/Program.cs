using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/GetGameTransaction/game/{gameTicketId}/transaction/{transactionId}", async (
    string gameTicketId,
    string transactionId,
    [FromServices] IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient();

    var gameTicketInfo = await httpClient.GetFromJsonAsync<object>($"http://localhost:5177/api/Game/get-ticket-info/{gameTicketId}");
    var transactionInfo = await httpClient.GetFromJsonAsync<object>($"http://localhost:5074/api/Payment/get-transaction-info/game/{gameTicketId}/transaction/{transactionId}");

    var result = new
    {
        gameTicketId,
        transactionId,
        gameTicketInfo,
        transactionInfo
    };

    return Results.Ok(result);
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
