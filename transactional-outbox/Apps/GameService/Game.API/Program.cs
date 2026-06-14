using Game.Application;
using Game.Infrastructure;
using Game.Infrastructure.SeedData;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(services =>
{
    InfrastructureServiceRegistration.AddInfrastructureServices(services, builder.Configuration);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5177", "http://localhost:8084")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobManager.AddOrUpdate<OutboxProcessor>(
        "ProcessOutboxMessages",
        processor => processor.ProcessOutboxMessagesAsync(CancellationToken.None),
        Cron.Minutely);
}


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    // PoC: create schema from the model. Replace with `db.Database.MigrateAsync()`
    // once `dotnet ef migrations add InitialCreate` has been run.
    await db.Database.EnsureCreatedAsync();
    await GameDbSeed.SeedAsync(db);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

try
{
    app.MapControllers();
}
catch (ReflectionTypeLoadException ex)
{
    foreach (var loaderException in ex.LoaderExceptions)
    {
        Console.WriteLine(loaderException?.Message);
    }
    throw;
}

app.Run();
