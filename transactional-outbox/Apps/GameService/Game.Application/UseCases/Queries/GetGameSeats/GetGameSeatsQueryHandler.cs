using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Queries.ViewModel;
using Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.GetGameSeats
{
    public class GetGameSeatsQueryHandler : IQueryHandler<GetGameSeatsQuery, List<GameSeatViewModel>>
    {
        private readonly IGameRepository _gameRepository;
        public GetGameSeatsQueryHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameSeatViewModel>> Handle(GetGameSeatsQuery query)
        {
            // Await the task to get the actual list of GameTicket objects
            var results = await _gameRepository.GetGameTicketsByGameId(query.GameId);

            // Map the results to GameSeatViewModel
            var gameSeats = results.Select(gt => new GameSeatViewModel
            {
                SeatId = gt.SeatId,
                IsAvailable = gt.Status == TicketStatus.Available,
                Price = gt.Price
            }).ToList();

            return gameSeats;
        }

    }
}
