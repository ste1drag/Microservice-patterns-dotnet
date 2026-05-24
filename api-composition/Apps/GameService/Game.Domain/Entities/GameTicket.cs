using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Domain.Enums;

namespace Game.Domain.Entities
{
    public class GameTicket
    {
        #region Properties
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid SeatId { get; set; }
        public int Price { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Available;
        public DateTime? ReservedAt { get; set; }
        public Guid? ReservationId { get; set; }
        public Game Game { get; set; }
        public StadiumSeat Seat { get; set; }
        #endregion
    }
}
