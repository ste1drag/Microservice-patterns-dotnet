using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.Contracts.Publisher
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken ct);
    }
}
