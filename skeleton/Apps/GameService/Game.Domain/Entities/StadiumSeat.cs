using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain.Entities
{
    public class StadiumSeat
    {
        #region Properties
        public Guid Id { get; set; }
        public Guid StadiumId { get; set; }
        public int Level { get; set; }
        public int SeatNumber { get; set; }
        public Stadium Stadium { get; set; }
        public GameTicket Ticket { get; set; }
        #endregion
    }
}
