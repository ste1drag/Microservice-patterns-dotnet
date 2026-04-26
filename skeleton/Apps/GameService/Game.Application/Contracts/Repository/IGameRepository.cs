using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Application.UseCases.Commands.DTO;
using Game.Domain.Entities;
using Game.Domain.Interfaces;

namespace Game.Application.Contracts.Repository
{
    public interface IGameRepository: IAsyncRepository<Domain.Entities.Game>
    {
        Task <List<Domain.Entities.Game>> GetGamesByStadiumId (Guid stadiumId);
        Task <List<GameTicket>> GetGameTicketsByGameId(Guid gameId);
        Task <GameInfoSeatModel> GetGameInfoSeat(Guid gameId, Guid seatId);
        Task<string> ExecuteTicketPayment(TicketSeatPaymentDTO ticketSeatPaymentDTO);
    }
}
