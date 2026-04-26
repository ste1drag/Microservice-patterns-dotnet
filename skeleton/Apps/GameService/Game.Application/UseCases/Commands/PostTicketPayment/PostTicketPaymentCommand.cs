using Game.Application.Interfaces;
using Game.Application.UseCases.Commands.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Commands.PostTicketPayment
{
    public class PostTicketPaymentCommand : ICommand<string>
    {
        public TicketSeatPaymentDTO TicketSeatPaymentDTO { get; set; }
    }
}
