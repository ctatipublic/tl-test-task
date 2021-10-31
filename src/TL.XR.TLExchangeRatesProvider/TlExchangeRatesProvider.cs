using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.TLExchangeRatesProvider
{
    public class TlExchangeRatesProvider : IExchangeRatesProvider
    {
        private readonly HttpClient _httpClient;
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public TlExchangeRatesProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(TlExchangeRatesProvider));
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsyncForIsoCurrencyAsync(string isoCurrency)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{isoCurrency.ToUpper()}.json");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tlxr = JsonSerializer.Deserialize<TlExchangeRateResponse>(content, _jsonSerializerOptions);
                    return tlxr.Rates;
                }
            }
            catch (Exception ex)
            { }

            return null;
        }
    }
}
