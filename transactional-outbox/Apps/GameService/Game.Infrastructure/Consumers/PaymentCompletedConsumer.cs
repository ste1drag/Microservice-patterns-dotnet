using Game.Application.Contracts.Repository;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;
using System.Threading.Tasks;

namespace Game.Infrastructure.Consumers
{
    public class PaymentCompletedConsumer : IConsumer<PaymentCompletedMessage>
    {
        private readonly IGameRepository _gameRepository;

        public PaymentCompletedConsumer(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedMessage> context)
        {
            var message = context.Message;

            if (message.Success)
            {
                var confirmed = await _gameRepository.ConfirmTicketAsync(message.GameTicketId);
            }
            else
            {
                var released = await _gameRepository.ReleaseTicketAsync(message.GameTicketId);
            }
        }
    }
}
