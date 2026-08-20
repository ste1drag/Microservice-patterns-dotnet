using MassTransit;
using Payment.Application.Contracts.Publisher;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<TMessage>(TMessage message, CancellationToken ct)
        {
            await _publishEndpoint.Publish(message, ct);
        }
    }
}
