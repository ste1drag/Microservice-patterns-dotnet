using Game.Application.UseCases.Commands.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.Contracts.Client
{
    public interface IPaymentClient
    {
        Task<PaymentResultDto> ExecutePaymentAsync(TicketSeatPaymentDTO paymentDto);
    }
}
