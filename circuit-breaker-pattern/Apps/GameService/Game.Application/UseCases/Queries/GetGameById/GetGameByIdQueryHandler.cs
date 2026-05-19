using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Queries.ViewModel;

namespace Game.Application.UseCases.Queries.GetGameById
{
    public class GetGameByIdQueryHandler : IQueryHandler<GetGameByIdQuery, GameViewModel>
    {
        private readonly IGameRepository _gameRepository;
        public GetGameByIdQueryHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<GameViewModel> Handle(GetGameByIdQuery query)
        {
            var game = await _gameRepository.GetGameByIdWithDetailsAsync(query.Id);

            if (game == null)
            {
                return null;
            }
            return new GameViewModel
            {
                Id = game.Id,
                HomeTeamName = game.HomeTeam?.Name,
                AwayTeamName = game.AwayTeam?.Name,
                StadiumName = game.GamePlace?.Name,
                Date = game.Date
            };
        }
    }
}
