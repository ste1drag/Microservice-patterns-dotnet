using Payment.Application.Contracts.Handlers;
using Payment.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.UseCases.Commands.PostExecutePaymentCommand
{
    class PostExecutePaymentCommandHandler : ICommandHandler<PostExecutePaymentCommand, string>
    {
        private readonly ITransactionRepository _transactionRepository;

        public PostExecutePaymentCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<string> Handle(PostExecutePaymentCommand command)
        {
            var result = await _transactionRepository.ExecutePayment(command.ticketSeatPaymentDTO);
            return result;
        }
    }
}
