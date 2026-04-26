using Game.Application.Interfaces;
using Game.Application.UseCases.Commands.DTO;
using Game.Application.UseCases.Commands.PostTicketPayment;
using Game.Application.UseCases.Queries.GetAllGames;
using Game.Application.UseCases.Queries.GetGameById;
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
            return Ok();
        }

        [HttpGet("get-game-by-id/{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            Guid guid = new Guid(id, 0, 0, new byte[8]);
            var query = new GetGameByIdQuery { Id = guid };
            var result = await _dispatcher.Query(query);
            return Ok();
        }

        [HttpGet("get-seat-info/game/{gameId}/seat/{seatId}")]
        public async Task<IActionResult> GetSeatInfo(Guid gameId, Guid seatId)
        {
            var query = new GetSeatInfoQuery { GameId = gameId, SeatId = seatId };
            var result = await _dispatcher.Query(query);
            return Ok(result);
        }
    }
}
