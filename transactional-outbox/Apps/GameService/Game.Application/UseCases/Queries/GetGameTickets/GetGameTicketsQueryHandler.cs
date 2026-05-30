using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Queries.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.GetGameTickets
{
    public class GetGameTicketsQueryHandler : IQueryHandler<GetGameTicketsQuery, List<GameTicketViewModel>>
    {
        private readonly IGameRepository _gameRepository;

        public GetGameTicketsQueryHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameTicketViewModel>> Handle(GetGameTicketsQuery query)
        {
            var tickets = await _gameRepository.GetGameTicketsByGameId(query.GameId);
            return tickets.Select(t => new GameTicketViewModel
            {
                Id = t.Id,
                GameId = t.GameId,
                SeatId = t.SeatId,
                Level = t.Seat.Level,
                ReservedAt = t.ReservedAt,
                Price = t.Price,
            }).ToList();
        }
    }
}
