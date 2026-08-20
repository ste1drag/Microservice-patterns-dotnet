using Payment.Application.Interfaces;
using Payment.Application.UseCases.Queries.ViewModel;

namespace Payment.Application.UseCases.Queries.GetTransactionInfo
{
    public class GetTransactionInfoQuery : IQuery<TransactionViewModel?>
    {
        public Guid TransactionId { get; set; }
        public Guid GameTicketId { get; set; }
    }
}
