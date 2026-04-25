using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain.Entities
{
    public class GameTicket
    {
        #region Properties
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid SeatId { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }
        public Game Game { get; set; }
        public StadiumSeat Seat { get; set; }
        #endregion
    }
}
