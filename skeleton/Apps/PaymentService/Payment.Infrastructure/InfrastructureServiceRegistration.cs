using Microsoft.EntityFrameworkCore; // Ensure this is included
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Contracts.Repositories;
using Payment.Application.Interfaces;
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


            services.AddScoped<ITransactionRepository, TransactionService>();
            services.AddScoped<IDispatcher, Dispatcher>();

            return services;
        }
    }
}
