using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Commands.DTO;
using Game.Domain.Enums;
using Shared.Events;

namespace Game.Application.UseCases.Commands.PostTicketPayment
{
    public class PostTicketPaymentCommandHandler : ICommandHandler<PostTicketPaymentCommand, PaymentResultDto>
    {
        private readonly IGameRepository _gameRepository;
        public PostTicketPaymentCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<PaymentResultDto> Handle(PostTicketPaymentCommand command)
        {
            var dto = command.TicketSeatPaymentDTO;
            if (dto.ReservationId == Guid.Empty) dto.ReservationId = Guid.NewGuid();

            var paymentRequested = new PaymentRequestedMessage
            {
                ReservationId = dto.ReservationId,
                GameTicketId = dto.GameTicketId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Currency = (int)dto.Currency
            };

            var reserved = await _gameRepository.ReserveTicketAndPublishAsync(
                dto.GameTicketId,
                dto.ReservationId,
                paymentRequested);

            if (!reserved)
            {
                return new PaymentResultDto
                {
                    GameTicketId = dto.GameTicketId,
                    Status = PaymentStatus.Failed,
                    Message = "Karta nije dostupna"
                };
            }

            return new PaymentResultDto
            {
                GameTicketId = dto.GameTicketId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                Status = PaymentStatus.Pending,
                Message = "Rezervacija uspešna, čekanje na plaćanje"
            };
        }
    }
}
