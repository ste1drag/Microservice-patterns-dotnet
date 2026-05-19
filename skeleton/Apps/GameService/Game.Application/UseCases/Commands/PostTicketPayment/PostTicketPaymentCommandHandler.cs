using Game.Application.Contracts.Client;
using Game.Application.Contracts.Handlers;
using Game.Application.Contracts.Repository;
using Game.Application.UseCases.Commands.DTO;
using Game.Domain.Enums;

namespace Game.Application.UseCases.Commands.PostTicketPayment
{
    public class PostTicketPaymentCommandHandler : ICommandHandler<PostTicketPaymentCommand, PaymentResultDto>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPaymentClient _paymentClient;
        public PostTicketPaymentCommandHandler(IGameRepository gameRepository, IPaymentClient paymentClient)
        {
            _gameRepository = gameRepository;
            _paymentClient = paymentClient;
        }

        public async Task<PaymentResultDto> Handle(PostTicketPaymentCommand command)
        {
            var dto = command.TicketSeatPaymentDTO;
            if (dto.ReservationId == Guid.Empty) dto.ReservationId = Guid.NewGuid();
            
            var reserved = await _gameRepository.TryReserveTicketAsync(dto.GameTicketId, dto.ReservationId);
            
            if (!reserved)
            {
                return new PaymentResultDto
                {
                    GameTicketId = dto.GameTicketId,
                    Status = PaymentStatus.Failed,
                    Message = "Ticket is not available"
                };
            }

            try
            {
                var result = await _paymentClient.ExecutePaymentAsync(dto);

                if (result.Status == PaymentStatus.Completed)
                    await _gameRepository.ConfirmTicketAsync(dto.GameTicketId);
                else
                    await _gameRepository.ReleaseTicketAsync(dto.GameTicketId);
                return result;
            }
            catch (Exception ex)
            {
                await _gameRepository.ReleaseTicketAsync(dto.GameTicketId);
                return new PaymentResultDto
                {
                    GameTicketId = dto.GameTicketId,
                    Status = PaymentStatus.Failed,
                    Message = $"Payment processing failed: {ex.Message}"
                };
            }
        }
    }
}
