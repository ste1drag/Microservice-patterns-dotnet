using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Domain.Enums;

namespace Game.Application.UseCases.Commands.DTO
{
    public class TicketSeatPaymentDTO
    {
        public Guid GameTicketId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentCurrency Currency { get; set; }
        public Guid ReservationId { get; set; }
    }
}
