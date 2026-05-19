using Game.Application.Contracts.Client;
using Game.Application.UseCases.Commands.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Game.Infrastructure.Clients
{
    public class PaymentHttpClient : IPaymentClient
    {
        private readonly HttpClient _httpClient;

        public PaymentHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaymentResultDto> ExecutePaymentAsync(TicketSeatPaymentDTO paymentDto)
        {
            var response = await _httpClient.PostAsJsonAsync("execute-payment", paymentDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaymentResultDto>();
            if (result == null)
            {
                throw new InvalidOperationException("Payment service returned empty response.");
            }

            return result;
        }
    }
}
