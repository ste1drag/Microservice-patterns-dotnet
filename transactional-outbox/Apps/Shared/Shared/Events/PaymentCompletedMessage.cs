using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class PaymentCompletedMessage
    {
        public Guid ReservationId { get; init; }
        public Guid GameTicketId { get; init; }
        public Guid TransactionId { get; init; }
        public bool Success { get; init; }
        public string? Message { get; init; }
    }
}
