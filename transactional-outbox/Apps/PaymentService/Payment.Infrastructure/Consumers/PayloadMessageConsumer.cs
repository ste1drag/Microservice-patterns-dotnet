using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Payment.Application.Contracts.Repositories;
using Shared.Events;

namespace Payment.Infrastructure.Consumers
{
    public class PayloadMessageConsumer : IConsumer<PaymentRequestedMessage>
    {
        private readonly ITransactionRepository _transactionRepository;
        public PayloadMessageConsumer() { 
        }

        public async Task Consume(ConsumeContext<PaymentRequestedMessage> context)
        {
            await _transactionRepository.ExecutePaymentWithOutboxAsync(context.Message);
        }
    }
}
