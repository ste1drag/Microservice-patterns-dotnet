using Game.Application.Interfaces;
using Game.Application.UseCases.Commands.DTO;
using Game.Application.UseCases.Commands.PostTicketPayment;
using Game.Application.UseCases.Queries.GetAllGames;
using Game.Application.UseCases.Queries.GetGameById;
using Game.Application.UseCases.Queries.GetGameSeats;
using Game.Application.UseCases.Queries.GetGameTicketInfo;
using Game.Application.UseCases.Queries.GetGameTickets;
using Game.Application.UseCases.Queries.GetSeatInfo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public GameController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("ticket-payment")]
        public async Task<IActionResult> PostTicketPayment([FromBody] TicketSeatPaymentDTO ticketSeatPaymentDTO)
        {
            var command = new PostTicketPaymentCommand { TicketSeatPaymentDTO = ticketSeatPaymentDTO };
            var result = await _dispatcher.Send(command);

            return Ok(result);
        }

        [HttpGet("get-all-games")]
        public async Task<IActionResult> GetAllGames()
        {
            var query = new GetAllGamesQuery();
            var result = await _dispatcher.Query(query);

            return Ok(result);
        }

        [HttpGet("get-game-seats/{id:guid}")]
        public async Task<IActionResult> GetGameSeats(Guid id)
        {
            var query = new GetGameSeatsQuery { GameId = id};
            var result = await _dispatcher.Query(query);

            return Ok(result);
        }

        [HttpGet("get-game-by-id/{id:guid}")]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            var query = new GetGameByIdQuery { Id = id };
            var result = await _dispatcher.Query(query);

            return Ok(result);
        }

        [HttpGet("get-game-tickets/{gameId:guid}")]
        public async Task<IActionResult> GetGameTickets(Guid gameId)
        {
            var query = new GetGameTicketsQuery { GameId = gameId };
            var result = await _dispatcher.Query(query);

            return Ok(result);
        }

        [HttpGet("get-ticket-info/{gameTicketId:guid}")]
        public async Task<IActionResult> GetGameTicketInfo(Guid gameTicketId)
        {
            var query = new GetGameTicketInfoQuery { TicketId = gameTicketId };
            var result = await _dispatcher.Query(query);
            return Ok(result);
        }

        [HttpGet("get-seat-info/game/{gameId:guid}/seat/{seatId:guid}")]
        public async Task<IActionResult> GetSeatInfo(Guid gameId, Guid seatId)
        {
            var query = new GetSeatInfoQuery { GameId = gameId, SeatId = seatId };
            var result = await _dispatcher.Query(query);

            return Ok(result);
        }
    }
}
