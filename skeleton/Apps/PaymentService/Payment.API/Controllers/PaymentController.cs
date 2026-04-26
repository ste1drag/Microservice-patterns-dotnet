using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Interfaces;
using Payment.Application.UseCases.Commands.DTO;
using Payment.Application.UseCases.Commands.PostExecutePaymentCommand;

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
    }
}
