using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Queries.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.GetAllGames
{
    public class GetAllGamesQueryHandler : IQueryHandler<GetAllGamesQuery, List<GameViewModel>>
    {
        private readonly IGameRepository _gameRepository;
        public GetAllGamesQueryHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<List<GameViewModel>> Handle(GetAllGamesQuery query)
        {
            var games = await _gameRepository.GetAllAsync();
            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                HomeTeamName = game.HomeTeam.Name,
                AwayTeamName = game.AwayTeam.Name,
                StadiumName = game.GamePlace.Name,
                Date = game.Date
            }).ToList();
        }
    }
}
