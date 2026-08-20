using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Contracts.Handlers;
using Payment.Application.UseCases.Commands.DTO;
using Payment.Application.UseCases.Commands.PostExecutePaymentCommand;
using Payment.Application.UseCases.Queries.GetTransactionInfo;
using Payment.Application.UseCases.Queries.ViewModel;
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
            services.AddScoped<IQueryHandler<GetTransactionInfoQuery, TransactionViewModel?>, GetTransactionInfoQueryHandler>();

            configureInfrastructureServices(services);

            return services;
        }
    }
}
