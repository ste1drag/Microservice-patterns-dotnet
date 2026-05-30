using Game.Application.Contracts.Repository;
using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Infrastructure.Services
{
    public class OutboxService : BaseService<OutboxMessage>, IOutboxRepository
    {
        public OutboxService(GameDbContext gameDbContext) : base(gameDbContext)
        {
        }

        public async Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(int batchSize)
        {
            if (batchSize <= 0)
            {
                throw new ArgumentException("Batch size must be greater than zero.", nameof(batchSize));
            }

            return await _gameDbContext.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(batchSize)
                .ToListAsync();
        }

        public async Task MarkProcessedAsync(Guid id)
        {
            var message = await _gameDbContext.OutboxMessages.FindAsync(id);
            if (message != null)
            {
                message.ProcessedAt = DateTime.UtcNow;
                _gameDbContext.OutboxMessages.Update(message);
                await _gameDbContext.SaveChangesAsync();
            }
        }
    }
}
