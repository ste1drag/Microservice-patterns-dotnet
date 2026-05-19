using Game.Application.Contracts.Client;
using Game.Application.Contracts.Repository;
using Game.Application.Interfaces;
using Game.Infrastructure.Clients;
using Game.Infrastructure.Services;
using Microsoft.EntityFrameworkCore; // Ensure this is included
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL; // Add this using directive

namespace Game.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("String connection not available");

            services.AddDbContext<GameDbContext>(options =>
                options.UseNpgsql(connectionString)); // Ensure the Npgsql package is installed

            var paymentServiceUrl = configuration["Services:PaymentServiceUrl"] ?? "http://localhost:5001/api/payment/";
            
            services.AddHttpClient<IPaymentClient, PaymentHttpClient>(client =>
            {
                client.BaseAddress = new Uri(paymentServiceUrl);
            });

            services.AddScoped<IGameRepository, GameService>();
            services.AddScoped<IStadiumRepository, StadiumService>();
            services.AddScoped<IDispatcher, Dispatcher>();


            return services;
        }
    }
}
