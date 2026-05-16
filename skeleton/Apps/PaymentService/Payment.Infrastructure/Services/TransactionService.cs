using Payment.Application.Contracts.Repositories;
using Payment.Application.UseCases.Commands.DTO;
using Payment.Domain.Entities;
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

        public Task<string> ExecutePayment(TicketSeatPaymentDTO ticketSeatPaymentDTO)
        {
            // Implement the logic to execute the payment using the provided TicketSeatPaymentDTO
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Status = Domain.Enums.PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _paymentDbcontext.Transactions.Add(transaction);
            _paymentDbcontext.SaveChanges();

            return Task.FromResult("Payment executed successfully");
        }
    }
}
