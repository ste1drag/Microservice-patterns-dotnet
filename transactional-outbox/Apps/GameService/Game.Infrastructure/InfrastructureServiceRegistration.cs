using Game.Application.Contracts.Client;
using Game.Application.Contracts.Publisher;
using Game.Application.Contracts.Repository;
using Game.Application.Interfaces;
using Game.Infrastructure.Clients;
using Game.Infrastructure.Consumers;
using Game.Infrastructure.Publisher;
using Game.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Game.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("String connection not available");

            var retryPolicy = GetRetryPolicy();
            var circuitBreakerPolicy = GetCircuitBreakerPolicy();

            services.AddDbContext<GameDbContext>(options =>
                options.UseNpgsql(connectionString)); // Ensure the Npgsql package is installed

            var paymentServiceUrl = configuration["Services:PaymentServiceUrl"] ?? "http://localhost:5001/api/payment/";
            
            services.AddHttpClient<IPaymentClient, PaymentHttpClient>(client =>
            {
                client.BaseAddress = new Uri(paymentServiceUrl);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PaymentCompletedConsumer>();

                x.AddEntityFrameworkOutbox<GameDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseEntityFrameworkOutbox<GameDbContext>(context);
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);  // auto-wire consumers if any
                });
            });

            services.AddScoped<IGameRepository, GameService>();
            services.AddScoped<IStadiumRepository, StadiumService>();
            services.AddScoped<IDispatcher, Dispatcher>();
            services.AddScoped<IMessagePublisher, MessagePublisher>();


            return services;
        }
    }
}
