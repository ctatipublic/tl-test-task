using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TL.XR.TLExchangeRatesProvider;
using Xunit;

namespace TL.XR.TLExchangeRatesProviderTests
{
    public class TlExchangeRatesProviderTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;

        public class MockHttpMessageHandler : DelegatingHandler
        {
            private readonly object _response;
            private readonly HttpStatusCode _responseStatusCode;
            private readonly bool _throwException;

            public MockHttpMessageHandler(object response, HttpStatusCode responseStatusCode, bool throwException)
            {
                _response = response;
                _responseStatusCode = responseStatusCode;
                _throwException = throwException;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (_throwException) { throw new Exception("Foo exception"); }

                var httpResponse = new HttpResponseMessage(_responseStatusCode)
                {
                    Content = new StringContent(JsonSerializer.Serialize(_response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json")
                };

                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(httpResponse);
                return tsc.Task;
            }
        }

        [Fact]
        public async Task GetExchangeRatesAsyncForIsoCurrencyAsync_Should_Return_Rates()
        {
            // Arrange
            var rates = new Dictionary<string, decimal>
            {
                { "GBP", 0.01234m },
                { "USD", 0.04321m },
                { "EUR", 0.05678m }
            };
            
            var httpResponse = new TlExchangeRateResponse
            {
                Base = "foobase",
                Date = new DateTime(),
                Rates = rates,
                TimeLastUpdated = 0
            };

            SetupHttpClientFactory(httpResponse, HttpStatusCode.OK, false);

            // Act
            var result = await new TlExchangeRatesProvider(_httpClientFactoryMock.Object).GetExchangeRatesAsyncForIsoCurrencyAsync("foo");

            // Assert
            Assert.Equal(rates, result);

        }

        [Fact]
        public async Task GetExchangeRatesAsyncForIsoCurrencyAsync_ShouldReturnNullIfNotSuccessCode()
        {
            // Arrange
            var rates = new Dictionary<string, decimal>
            {
                { "GBP", 0.01234m },
                { "USD", 0.04321m },
                { "EUR", 0.05678m }
            };

            var httpResponse = new TlExchangeRateResponse
            {
                Base = "foobase",
                Date = new DateTime(),
                Rates = rates,
                TimeLastUpdated = 0
            };

            SetupHttpClientFactory(httpResponse, HttpStatusCode.InternalServerError, false);

            // Act
            var result = await new TlExchangeRatesProvider(_httpClientFactoryMock.Object).GetExchangeRatesAsyncForIsoCurrencyAsync("foo");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExchangeRatesAsyncForIsoCurrencyAsync_ShouldReturnNullOnException()
        {
            // Arrange
            var rates = new Dictionary<string, decimal>
            {
                { "GBP", 0.01234m },
                { "USD", 0.04321m },
                { "EUR", 0.05678m }
            };

            var httpResponse = new TlExchangeRateResponse
            {
                Base = "foobase",
                Date = new DateTime(),
                Rates = rates,
                TimeLastUpdated = 0
            };

            SetupHttpClientFactory(httpResponse, HttpStatusCode.OK, true);

            // Act
            var result = await new TlExchangeRatesProvider(_httpClientFactoryMock.Object).GetExchangeRatesAsyncForIsoCurrencyAsync("foo");

            // Assert
            Assert.Null(result);
        }

        private void SetupHttpClientFactory(object response, HttpStatusCode responseStatusCode, bool throwException)
        {
            var httpClient = new HttpClient(new MockHttpMessageHandler(response, responseStatusCode, throwException));
            httpClient.BaseAddress = new Uri("https://foo.bar", UriKind.Absolute);
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock
                .Setup(m => m.CreateClient(nameof(TlExchangeRatesProvider)))
                .Returns(httpClient);
        }
    }
}
