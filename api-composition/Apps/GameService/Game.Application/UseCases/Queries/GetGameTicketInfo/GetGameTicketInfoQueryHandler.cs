using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Queries.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.GetGameTicketInfo
{
    public class GetGameTicketInfoQueryHandler : IQueryHandler<GetGameTicketInfoQuery, GameTicketViewModel>
    {
        private readonly IGameRepository _gameRepository;

        public GetGameTicketInfoQueryHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameTicketViewModel> Handle(GetGameTicketInfoQuery request)
        {
            var ticket = await _gameRepository.GetGameTicketByIdAsync(request.TicketId);
            
            if (ticket == null)
            {
                return null;
            }

            return new GameTicketViewModel
            {
                Id = ticket.Id,
                GameId = ticket.GameId,
                SeatId = ticket.SeatId,
                Price = ticket.Price,
                Status = ticket.Status.ToString(),
                Level = ticket.Seat.Level,
                ReservedAt = ticket.ReservedAt
            };
        }
    }
}
