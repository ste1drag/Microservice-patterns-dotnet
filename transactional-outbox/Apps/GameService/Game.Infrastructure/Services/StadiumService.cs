using Game.Application.Contracts.Repository;
using Game.Domain.Entities;
namespace Game.Infrastructure.Services
{
    public class StadiumService : BaseService<Stadium>, IStadiumRepository
    {
        public StadiumService(GameDbContext gameDbContext): base(gameDbContext)
        {

        }

        public Task<List<StadiumSeat>> GetStadiumSeatsByStadiumId(string stadiumId)
        {
            var seats = _gameDbContext.StadiumSeats.Where(s => s.StadiumId.ToString() == stadiumId).ToList();
            return Task.FromResult(seats);
        }

        public Task<bool> IsGameSeatAvailable(string stadiumSeatId, string gameId)
        {
            var gameTicket = _gameDbContext.GameTickets.FirstOrDefault(gt => gt.SeatId.ToString() == stadiumSeatId && gt.GameId.ToString() == gameId);
            return Task.FromResult(gameTicket == null);
        }

    }
}
