using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Contracts.Handlers;
using Payment.Application.UseCases.Commands.DTO;
using Payment.Application.UseCases.Commands.PostExecutePaymentCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Action<IServiceCollection> configureInfrastructureServices)
        {

            services.AddScoped<ICommandHandler<PostExecutePaymentCommand, PaymentResultDto>, PostExecutePaymentCommandHandler>();
            // Register application services, handlers, etc. here
            // For example:
            // services.AddScoped<IYourService, YourServiceImplementation>();

            configureInfrastructureServices(services);

            return services;
        }
    }
}
