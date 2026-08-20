using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Interfaces;
using Payment.Application.UseCases.Commands.DTO;
using Payment.Application.UseCases.Commands.PostExecutePaymentCommand;
using Payment.Application.UseCases.Queries.GetTransactionInfo;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public PaymentController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("execute-payment")]
        public async Task<IActionResult> ExecutePayment([FromBody] TicketSeatPaymentDTO ticketSeatPaymentDTO)
        {
            var command = new PostExecutePaymentCommand { TicketSeatPaymentDTO = ticketSeatPaymentDTO };
            var result = await _dispatcher.Send(command);
            return Ok(result);
        }

        [HttpGet("get-transaction-info/game/{gameTicketId:guid}/transaction/{transactionId:guid}")]
        public async Task<IActionResult> GetTransactionInfo(Guid gameTicketId, Guid transactionId)
        {
            var query = new GetTransactionInfoQuery
            {
                GameTicketId = gameTicketId,
                TransactionId = transactionId
            };

            var result = await _dispatcher.Query(query);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
