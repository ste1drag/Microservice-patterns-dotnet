using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payment.Infrastructure.Consumers;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Payment.Infrastructure
{
    public sealed class OutboxProcessor
    {
        private const int BatchSize = 50;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task ProcessOutboxMessagesAsync(CancellationToken ct = default)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
            var consumer = scope.ServiceProvider.GetRequiredService<PayloadMessageConsumer>();

            var outboxMessages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(BatchSize)
                .ToListAsync(ct);

            foreach (var message in outboxMessages)
            {
                try
                {
                    if (message.Type == nameof(PaymentRequestedMessage))
                    {
                        var evt = JsonSerializer.Deserialize<PaymentRequestedMessage>(message.Payload)!;
                        var consumeContext = new ConsumeContextWrapper<PaymentRequestedMessage>(evt);
                        await consumer.Consume(consumeContext);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unknown outbox message type: {message.Type}");
                    }

                    message.ProcessedAt = DateTime.UtcNow;
                    message.Error = null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Outbox message {OutboxId} of type {Type} failed to publish",
                        message.Id, message.Type);
                    message.Error = ex.Message;
                }
            }

            await dbContext.SaveChangesAsync(ct);
        }
    }
}
