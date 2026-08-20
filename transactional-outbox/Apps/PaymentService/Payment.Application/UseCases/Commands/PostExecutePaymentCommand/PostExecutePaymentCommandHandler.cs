using Payment.Application.Contracts.Handlers;
using Payment.Application.Contracts.Repositories;
using Payment.Application.UseCases.Commands.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.UseCases.Commands.PostExecutePaymentCommand
{
    public class PostExecutePaymentCommandHandler : ICommandHandler<PostExecutePaymentCommand, PaymentResultDto>
    {
        private readonly ITransactionRepository _transactionRepository;

        public PostExecutePaymentCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<PaymentResultDto> Handle(PostExecutePaymentCommand command)
        {
            var result = await _transactionRepository.ExecutePayment(command.TicketSeatPaymentDTO);
            return result;
        }
    }
}
