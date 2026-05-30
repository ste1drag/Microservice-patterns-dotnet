using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public record PaymentRequestedMessage
    {
        public Guid ReservationId { get; init; }
        public Guid GameTicketId { get; init; }
        public Guid UserId { get; init; }
        public decimal Amount { get; init; }
        public int Currency { get; init; }  // enum as int
    }
}
