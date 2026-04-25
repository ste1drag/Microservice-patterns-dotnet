using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain.Interfaces
{
    public class GameInfoSeatModel
    {
        public Guid GameId { get; set; }
        public Guid SeatId { get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
        public int Message { get; set; }
        public int Level { get; set; }
        public int SeatNumber { get; set; }
    }
}
