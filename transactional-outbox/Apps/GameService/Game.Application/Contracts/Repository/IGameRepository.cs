using Game.Application.UseCases.Commands.DTO;
using Game.Domain.Entities;
using Game.Domain.Interfaces;
using Shared.Events;

namespace Game.Application.Contracts.Repository
{
    public interface IGameRepository: IAsyncRepository<Domain.Entities.Game>
    {
        Task<List<Domain.Entities.Game>> GetAllGamesWithDetailsAsync();
        Task<Domain.Entities.Game?> GetGameByIdWithDetailsAsync(Guid id);
        Task <List<Domain.Entities.Game>> GetGamesByStadiumId (Guid stadiumId);
        Task <List<GameTicket>> GetGameTicketsByGameId(Guid gameId);
        Task <GameInfoSeatModel> GetGameInfoSeat(Guid gameId, Guid seatId);
        Task<bool> TryReserveTicketAsync(Guid ticketId, Guid reservationId);
        Task<bool> ConfirmTicketAsync(Guid ticketId);
        Task<bool> ReleaseTicketAsync(Guid ticketId);
        Task<GameTicket> GetGameTicketByIdAsync(Guid ticketId);

        Task<bool> ReserveTicketAndPublishAsync(
            Guid ticketId,
            Guid reservationId,
            PaymentRequestedMessage message);
    }
}
