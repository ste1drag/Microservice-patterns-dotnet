using Payment.Application.Contracts.Handlers;
using Payment.Application.Contracts.Repositories;
using Payment.Application.UseCases.Queries.ViewModel;

namespace Payment.Application.UseCases.Queries.GetTransactionInfo
{
    public class GetTransactionInfoQueryHandler : IQueryHandler<GetTransactionInfoQuery, TransactionViewModel?>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionInfoQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionViewModel?> Handle(GetTransactionInfoQuery query)
        {
            var transaction = await _transactionRepository.GetByIdAndTicketAsync(query.TransactionId, query.GameTicketId);
            if (transaction == null)
            {
                return null;
            }

            return new TransactionViewModel
            {
                TransactionId = transaction.Id,
                GameTicketId = transaction.GameTicketId,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
    }
}
