using Game.Application.Contracts.Handlers;
using Game.Application.UseCases.Commands.DTO;
using Game.Application.UseCases.Commands.PostTicketPayment;
using Game.Application.UseCases.Queries.GetAllGames;
using Game.Application.UseCases.Queries.GetGameById;
using Game.Application.UseCases.Queries.GetGameSeats;
using Game.Application.UseCases.Queries.GetGameTicketInfo;
using Game.Application.UseCases.Queries.GetGameTickets;
using Game.Application.UseCases.Queries.GetSeatInfo;
using Game.Application.UseCases.Queries.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, Action<IServiceCollection> configureInfrastructureServices)
        {
            services.AddScoped<ICommandHandler<PostTicketPaymentCommand, PaymentResultDto>, PostTicketPaymentCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllGamesQuery, List<GameViewModel>>, GetAllGamesQueryHandler>();
            services.AddScoped<IQueryHandler<GetGameByIdQuery, GameViewModel>, GetGameByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetSeatInfoQuery, GameSeatViewModel>, GetSeatInfoQueryHandler>();
            services.AddScoped<IQueryHandler<GetGameSeatsQuery, List<GameSeatViewModel>>, GetGameSeatsQueryHandler>();
            services.AddScoped<IQueryHandler<GetGameTicketsQuery, List<GameTicketViewModel>>, GetGameTicketsQueryHandler>();
            services.AddScoped<IQueryHandler<GetGameTicketInfoQuery, GameTicketViewModel>, GetGameTicketInfoQueryHandler>();

            configureInfrastructureServices(services);

            return services;
        }
    }
}
