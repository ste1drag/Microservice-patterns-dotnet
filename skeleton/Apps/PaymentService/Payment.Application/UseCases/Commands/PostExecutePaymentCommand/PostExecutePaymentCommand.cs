using Payment.Application.Interfaces;
using Payment.Application.UseCases.Commands.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.UseCases.Commands.PostExecutePaymentCommand
{
    public class PostExecutePaymentCommand: ICommand<string>
    {
        public TicketSeatPaymentDTO TicketSeatPaymentDTO{ get; set; }
    }
}
