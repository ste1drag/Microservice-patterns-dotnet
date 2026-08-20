using Game.Application.Contracts.Client;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Commands.DTO;
using Game.Domain.Entities;
using Game.Domain.Enums;
using Game.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Game.Infrastructure.Services
{
    public class GameService: BaseService<Domain.Entities.Game>, IGameRepository
    {

        public async Task<List<Domain.Entities.Game>> GetAllGamesWithDetailsAsync()
        {
            return await _gameDbContext.Games
                .Include(g => g.HomeTeam)
                .Include(g => g.AwayTeam)
                .Include(g => g.GamePlace)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Game?> GetGameByIdWithDetailsAsync(Guid id)
        {
            return await _gameDbContext.Games
                .Include(g => g.HomeTeam)
                .Include(g => g.AwayTeam)
                .Include(g => g.GamePlace)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public GameService(GameDbContext gameDbContext) : base(gameDbContext)
        {
        }

        public async Task<List<Domain.Entities.Game>> GetGamesByStadiumId(Guid stadiumId)
        {
            var results = await _gameDbContext.Games
                .Where(g => g.StadiumId == stadiumId)
                .Include(g => g.HomeTeam)
                .Include(g => g.AwayTeam)
                .Include(g => g.GamePlace)
                .ToListAsync();

            return results;
        }

        public async Task<List<GameTicket>> GetGameTicketsByGameId(Guid gameId)
        {
            var results = await _gameDbContext.GameTickets
                .Where(gt => gt.GameId == gameId)
                .Include(gt => gt.Game)
                .Include(gt => gt.Seat)
                .ToListAsync();

            if (results == null || results.Count == 0)
            {
                return new List<GameTicket>();
            }

            return results;
        }

        public async Task<GameInfoSeatModel> GetGameInfoSeat(Guid gameId, Guid seatId)
        {
            var result = await _gameDbContext.GameTickets
                .Where(gt => gt.GameId == gameId && gt.SeatId == seatId)
                .Include(gt => gt.Seat)
                .Select(gt => new GameInfoSeatModel
                {
                    GameId = gt.GameId,
                    SeatId = gt.SeatId,
                    IsAvailable = gt.Status == TicketStatus.Available,
                    Price = gt.Price,
                    Message = gt.Status == TicketStatus.Available ? "Dostupno za kupovinu" : "Nije dostupno za kupovinu",
                    Level = gt.Seat.Level,
                    SeatNumber = gt.Seat.SeatNumber
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return new GameInfoSeatModel
                {
                    GameId = gameId,
                    SeatId = seatId,
                    IsAvailable = false,
                    Price = 0,
                    Message = "Nije dostupno za kupovinu",
                    Level = 0,
                    SeatNumber = 0
                };
            }

            return result;
        }

        public async Task<bool> TryReserveTicketAsync(Guid ticketId, Guid reservationId)
        {
            var updated = await _gameDbContext.GameTickets
                .Where(gt => gt.Id == ticketId && gt.Status == TicketStatus.Available)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(gt => gt.Status, TicketStatus.Reserved)
                    .SetProperty(gt => gt.ReservedAt, DateTime.UtcNow)
                    .SetProperty(gt => gt.ReservationId, reservationId)
                );

            return updated > 0;
        }

        public async Task<bool> ConfirmTicketAsync(Guid ticketId)
        {
            var updated = await _gameDbContext.GameTickets
                .Where(gt => gt.Id == ticketId && gt.Status == TicketStatus.Reserved)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(gt => gt.Status, TicketStatus.Sold)
                );

            return updated > 0;
        }

        public async Task<bool> ReleaseTicketAsync(Guid ticketId)
        {
            var updated = await _gameDbContext.GameTickets
                .Where(gt => gt.Id == ticketId && gt.Status == TicketStatus.Reserved)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(gt => gt.Status, TicketStatus.Available)
                    .SetProperty(gt => gt.ReservedAt, (DateTime?)null)
                    .SetProperty(gt => gt.ReservationId, (Guid?)null)
                );

            return updated > 0;
        }
    }
}
