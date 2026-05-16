using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Commands.DTO;
using Game.Domain.Entities;
using Game.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Game.Infrastructure.Services
{
    public class GameService: BaseService<Domain.Entities.Game>, IGameRepository
    {
        public GameService(GameDbContext gameDbContext) : base(gameDbContext)
        {
        }

        public async Task<List<Domain.Entities.Game>> GetGamesByStadiumId(Guid stadiumId)
        {
            var results = await _gameDbContext.Games.Where(g => g.StadiumId == stadiumId).ToListAsync();

            return results;
        }


        public async Task<List<GameTicket>> GetGameTicketsByGameId(Guid gameId)
        {
            var results = await _gameDbContext.GameTickets.Where(gt => gt.GameId == gameId).ToListAsync();

            if (results == null || results.Count == 0)
            {
                return new List<GameTicket>();
            }

            return results;
        }

        public async Task<GameInfoSeatModel> GetGameInfoSeat(Guid gameId, Guid seatId)
        {
            var result = _gameDbContext.GameTickets
                .Where(gt => gt.GameId == gameId && gt.SeatId == seatId)
                .Include(gt => gt.Seat) // Include the Seat navigation property to access SeatNumber and Level
                .Select(gt => new GameInfoSeatModel
                {
                    GameId = gt.GameId,
                    SeatId = gt.SeatId,
                    IsAvailable = true,
                    Price = gt.Price,
                    Message =  "Dostupno za kupovinu",
                    Level = gt.Seat.Level,
                    SeatNumber = gt.Seat.SeatNumber
                })
                .FirstOrDefault();

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

        public async Task<string> ExecuteTicketPayment(TicketSeatPaymentDTO ticketSeatPaymentDTO)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.ticketpay.com/v1/");

            var response = await client.PostAsJsonAsync<TicketSeatPaymentDTO>("/ticketpay", ticketSeatPaymentDTO);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<String>();
                return responseData;
            }
            else
            {
                // Handle error response
                return "Error: " + response.StatusCode;
            }
        }

        public async Task<string> ConfirmTicketPayment(TicketSeatPaymentDTO ticketSeatPaymentDTO)
        {
            // Simulate confirmation logic
            return "Payment confirmed for ticket: " + ticketSeatPaymentDTO.GameId;
        }

    }
}
