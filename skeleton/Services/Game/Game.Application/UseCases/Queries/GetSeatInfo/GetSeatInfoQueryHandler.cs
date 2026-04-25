using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Queries.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.GetSeatInfo
{
    class GetSeatInfoQueryHandler : IQueryHandler<GetSeatInfoQuery, GameSeatViewModel>
    {
        private readonly IGameRepository _gameService;
        public GetSeatInfoQueryHandler(IGameRepository gameService)
        {
            _gameService = gameService;
        }
        public async Task<GameSeatViewModel> Handle(GetSeatInfoQuery query)
        {
            var seatInfo = await _gameService.GetGameInfoSeat(query.GameId, query.SeatId);
            if (seatInfo == null)
            {
                return null;
            }
            return new GameSeatViewModel
            {
                IsAvailable = seatInfo.IsAvailable,
                Price = seatInfo.Price,
                Message = seatInfo.Message,
                Level = seatInfo.Level,
                SeatNumber = seatInfo.SeatNumber
            };
        }
    }
}
