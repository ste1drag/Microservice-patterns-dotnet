using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Queries.ViewModel
{
    public class GameTicketViewModel
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid SeatId { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
        public int Level { get; set; }
        public DateTime? ReservedAt { get; set; }
    }
}
