using Payment.Application.UseCases.Commands.DTO;
using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Contracts.Repositories
{
    public interface ITransactionRepository : IAsyncRepository<Transaction>
    {
        Task<PaymentResultDto> ExecutePayment(TicketSeatPaymentDTO ticketSeatPaymentDTO);
        Task<Transaction?> GetByIdAndTicketAsync(Guid transactionId, Guid gameTicketId);
    }
}
