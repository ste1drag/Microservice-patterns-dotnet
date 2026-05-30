using Payment.Domain.Enums;

namespace Payment.Application.UseCases.Queries.ViewModel
{
    public class TransactionViewModel
    {
        public Guid TransactionId { get; set; }
        public Guid GameTicketId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentCurrency Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
