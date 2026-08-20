using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Contracts.Publisher;
using Payment.Application.Contracts.Repositories;
using Payment.Application.Interfaces;
using Payment.Infrastructure.Consumers;
using Payment.Infrastructure.Publisher;
using Payment.Infrastructure.Services;

namespace Payment.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("String connection not available");
            // Register the PaymentDbContext with the connection string from configuration
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PayloadMessageConsumer>();

                x.AddEntityFrameworkOutbox<PaymentDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseEntityFrameworkOutbox<PaymentDbContext>(context);
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

            services.AddScoped<ITransactionRepository, TransactionService>();
            services.AddScoped<IDispatcher, Dispatcher>();
            services.AddScoped<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}
