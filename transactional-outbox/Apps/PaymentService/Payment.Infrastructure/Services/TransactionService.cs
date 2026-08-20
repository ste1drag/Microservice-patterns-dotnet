using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Contracts.Repositories;
using Payment.Application.UseCases.Commands.DTO;
using Payment.Domain.Entities;
using Payment.Domain.Enums;
using Shared.Events;

namespace Payment.Infrastructure.Services
{
    public class TransactionService: BaseService<Transaction>, ITransactionRepository
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TransactionService(PaymentDbContext paymentDbContext, IPublishEndpoint publishEndpoint)
            : base(paymentDbContext)
        {
            _publishEndpoint = publishEndpoint;
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

        public async Task ExecutePaymentWithOutboxAsync(PaymentRequestedMessage message)
        {
            var status = message.Amount <= 0 ? PaymentStatus.Failed : PaymentStatus.Completed;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                GameTicketId = message.GameTicketId,
                UserId = message.UserId,
                Amount = message.Amount,
                Currency = (PaymentCurrency)message.Currency,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _paymentDbcontext.Transactions.Add(transaction);

            var paymentCompleted = new PaymentCompletedMessage
            {
                ReservationId = message.ReservationId,
                GameTicketId = message.GameTicketId,
                TransactionId = transaction.Id,
                Success = status == PaymentStatus.Completed,
                Message = status == PaymentStatus.Completed
                    ? "Payment executed successfully"
                    : "Payment failed (invalid amount)"
            };

            await _publishEndpoint.Publish(paymentCompleted);
            await _paymentDbcontext.SaveChangesAsync();
        }

        public Task<Transaction?> GetByIdAndTicketAsync(Guid transactionId, Guid gameTicketId)
        {
            return _paymentDbcontext.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.GameTicketId == gameTicketId);
        }
    }
}
