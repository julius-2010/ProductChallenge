using ProductChallenge.Application.Abstractions.Services;
using ProductChallenge.Application.Common.Exceptions;
using ProductChallenge.Infrastructure.Services.Models;
using System.Net;
using System.Net.Http.Json;

namespace ProductChallenge.Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetDiscountByProductIdAsync(int productId, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync($"discounts/{productId}", cancellationToken);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                throw new ExternalServiceException("El servicio de descuentos no está disponible.");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ExternalServiceException($" No se encontró ningún descuento para el producto {productId}.");

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException("El servicio de descuentos devolvió una respuesta inesperada.");

            var result = await response.Content.ReadFromJsonAsync<DiscountApiResponse>(cancellationToken: cancellationToken);

            if (result is null)
                throw new ExternalServiceException("El servicio de descuentos devolvió una respuesta vacía.");

            if (result.Discount < 0 || result.Discount > 100)
                throw new ExternalServiceException("El valor del descuento está fuera del rango válido [0-100].");

            return result.Discount;
        }
    }
}
