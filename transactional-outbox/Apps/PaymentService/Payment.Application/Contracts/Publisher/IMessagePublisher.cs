using System.Threading;
using System.Threading.Tasks;

namespace Payment.Application.Contracts.Publisher
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken ct);
    }
}
