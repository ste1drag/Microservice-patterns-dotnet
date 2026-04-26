using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Commands.DTO
{
    public class TicketSeatPaymentDTO
    {
        public string GameId { get; set; }
        public string StadiumSeatId { get; set; }
        public decimal Price { get; set; }
    }
}
