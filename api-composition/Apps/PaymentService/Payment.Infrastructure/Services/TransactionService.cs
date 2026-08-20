using Microsoft.EntityFrameworkCore;
using Payment.Application.Contracts.Repositories;
using Payment.Application.UseCases.Commands.DTO;
using Payment.Domain.Entities;
using Payment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Services
{
    public class TransactionService: BaseService<Transaction>, ITransactionRepository
    {
        public TransactionService(PaymentDbContext paymentDbContext) : base(paymentDbContext)
        {
        }

        public async Task<PaymentResultDto> ExecutePayment(TicketSeatPaymentDTO ticketSeatPaymentDTO)
        { 
            var status = ticketSeatPaymentDTO.Amount <= 0 ? PaymentStatus.Failed : PaymentStatus.Completed;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                GameTicketId = ticketSeatPaymentDTO.GameTicketId,
                UserId = ticketSeatPaymentDTO.UserId,
                Amount = ticketSeatPaymentDTO.Amount,
                Currency = ticketSeatPaymentDTO.Currency,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _paymentDbcontext.Transactions.Add(transaction);
            await _paymentDbcontext.SaveChangesAsync();

            return new PaymentResultDto
            {
                TransactionId = transaction.Id,
                GameTicketId = transaction.GameTicketId,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt,
                Message = status == PaymentStatus.Completed
        ? "Payment executed successfully"
        : "Payment failed (invalid amount)"
            };
        }

        public Task<Transaction?> GetByIdAndTicketAsync(Guid transactionId, Guid gameTicketId)
        {
            return _paymentDbcontext.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.GameTicketId == gameTicketId);
        }
    }
}
