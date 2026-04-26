using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.UseCases.Commands.PostTicketPayment
{
    class PostTicketPaymentCommandHandler : ICommandHandler<PostTicketPaymentCommand, string>
    {
        private readonly IGameRepository _gameRepository;
        public PostTicketPaymentCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<string> Handle(PostTicketPaymentCommand command)
        {
            var result = await _gameRepository.ExecuteTicketPayment(command.TicketSeatPaymentDTO);
            return result;
        }
    }
}
