using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payment.Domain.Enums;

namespace Payment.Application.UseCases.Commands.DTO
{
    public class PaymentResultDto
    {
        public Guid TransactionId { get; set; }
        public Guid GameTicketId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentCurrency Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; }
    }
}
