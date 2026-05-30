using Game.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Application.Contracts.Repository
{
    public interface IOutboxRepository : IAsyncRepository<OutboxMessage>
    {
        Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(int batchSize);
        Task MarkProcessedAsync(Guid id);
    }
}
