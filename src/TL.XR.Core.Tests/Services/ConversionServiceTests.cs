using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.XR.Core.Services;
using TL.XR.Infrastructure.DomainModel;
using TL.XR.Infrastructure.Interfaces;
using Xunit;

namespace TL.XR.Core.Tests.Services
{
    public class ConversionServiceTests
    {
        private List<Mock<IValidator<CurrencyConversion>>> _currencyConversionValidatorMocks;
        private Mock<IExchangeRatesProvider> _exchangeRatesProviderMock;

        [Fact]
        public async Task ConvertAsync_Returns_AllValidationErrorsAndOriginalPayload_WhenValidationError()
        {
            // Arrange
            SetupDefaultMocks();
            _currencyConversionValidatorMocks.ElementAt(0).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[] { "Error from validator1" });

            _currencyConversionValidatorMocks.ElementAt(1).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[] { "Error from validator2" });

            var currencyConversion = new CurrencyConversion()
            {
                From = new AmountInCurrency { Amount = 1, Currency = "CUR1" },
                To = new AmountInCurrency { Amount = 2, Currency = "CUR2" },
            };

            // Act
            var service = GetService();
            var result = await service.ConvertAsync(currencyConversion);

            // Assert
            Assert.Equal(2, result.Errors.Count());
            Assert.Equal("Error from validator1", result.Errors.ElementAt(0));
            Assert.Equal("Error from validator2", result.Errors.ElementAt(1));
            _exchangeRatesProviderMock.Verify(m => m.GetExchangeRatesAsyncForIsoCurrencyAsync(It.IsAny<string>()), Times.Never());
            Assert.Equal(result.From, currencyConversion.From);
            Assert.Equal(result.To, currencyConversion.To);
        }

        [Fact]
        public async Task ConvertAsync_Returns_ErrorAndOriginalPayload_ProviderReturnsNull()
        {
            // Arrange
            SetupDefaultMocks();
            _currencyConversionValidatorMocks.ElementAt(0).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[0]);

            _currencyConversionValidatorMocks.ElementAt(1).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[0]);

            _exchangeRatesProviderMock.Setup(m =>
                m.GetExchangeRatesAsyncForIsoCurrencyAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null);

            var currencyConversion = new CurrencyConversion()
            {
                From = new AmountInCurrency { Amount = 1, Currency = "CUR1" },
                To = new AmountInCurrency { Amount = 2, Currency = "CUR2" },
            };

            // Act
            var service = GetService();
            var result = await service.ConvertAsync(currencyConversion);

            // Assert
            Assert.Single(result.Errors);
            Assert.Equal(ConversionService.Errors.UnableToConvertError, result.Errors.ElementAt(0));
            _exchangeRatesProviderMock.Verify(m => m.GetExchangeRatesAsyncForIsoCurrencyAsync(It.IsAny<string>()), Times.Once());
            Assert.Equal(result.From, currencyConversion.From);
            Assert.Equal(result.To, currencyConversion.To);
        }

        [Theory]
        [InlineData("CUR1")]
        [InlineData("CUR3")]
        public async Task ConvertAsync_Returns_ErrorAndOriginalPayload_ProviderDoesNotReturnsCurrency(string providerResponseCurrencyName)
        {
            // Arrange
            SetupDefaultMocks();
            _currencyConversionValidatorMocks.ElementAt(0).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[0]);

            _currencyConversionValidatorMocks.ElementAt(1).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[0]);

            _exchangeRatesProviderMock.Setup(m =>
                m.GetExchangeRatesAsyncForIsoCurrencyAsync(It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<string, decimal> { { providerResponseCurrencyName, 0.001m } });

            var currencyConversion = new CurrencyConversion()
            {
                From = new AmountInCurrency { Amount = 1, Currency = "CUR1" },
                To = new AmountInCurrency { Amount = 2, Currency = "CUR2" },
            };

            // Act
            var service = GetService();
            var result = await service.ConvertAsync(currencyConversion);

            // Assert
            Assert.Single(result.Errors);
            Assert.Equal(ConversionService.Errors.UnableToConvertError, result.Errors.ElementAt(0));
            _exchangeRatesProviderMock.Verify(m => m.GetExchangeRatesAsyncForIsoCurrencyAsync(It.IsAny<string>()), Times.Once());
            Assert.Equal(result.From, currencyConversion.From);
            Assert.Equal(result.To, currencyConversion.To);
        }

        [Fact]
        public async Task ConvertAsync_Returns_CorrectResult()
        {
            // Arrange
            SetupDefaultMocks();
            _currencyConversionValidatorMocks.ElementAt(0).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[0]);

            _currencyConversionValidatorMocks.ElementAt(1).Setup(m =>
                m.ValidateAsync(It.IsAny<CurrencyConversion>()))
                .ReturnsAsync(new string[0]);

            _exchangeRatesProviderMock.Setup(m =>
                m.GetExchangeRatesAsyncForIsoCurrencyAsync(It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<string, decimal> { { "CUR2", 0.10m } });

            var currencyConversion = new CurrencyConversion()
            {
                From = new AmountInCurrency { Amount = 2, Currency = "CUR1" },
                To = new AmountInCurrency { Amount = 1, Currency = "CUR2" },
            };

            // Act
            var service = GetService();
            var result = await service.ConvertAsync(currencyConversion);

            // Assert
            _exchangeRatesProviderMock.Verify(m => m.GetExchangeRatesAsyncForIsoCurrencyAsync(It.IsAny<string>()), Times.Once());
            Assert.Equal(result.From, currencyConversion.From);
            Assert.Equal(result.To, currencyConversion.To);
            Assert.Equal(.2m, result.To.Amount);
        }



        private void SetupDefaultMocks()
        {
            _currencyConversionValidatorMocks = new List<Mock<IValidator<CurrencyConversion>>>
            {
                new Mock<IValidator<CurrencyConversion>>(),
                new Mock<IValidator<CurrencyConversion>>()
            };

            _exchangeRatesProviderMock = new Mock<IExchangeRatesProvider>();
        }
        private ConversionService GetService()
        {
            return new ConversionService(_currencyConversionValidatorMocks.Select(m => m.Object), _exchangeRatesProviderMock.Object);
        }
    }
}
