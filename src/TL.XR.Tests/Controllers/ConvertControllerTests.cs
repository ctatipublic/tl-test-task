using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TL.XR.Controllers;
using TL.XR.Infrastructure.DomainModel;
using TL.XR.Infrastructure.Interfaces;
using Xunit;

namespace TL.XR.Tests
{
    public class ConvertControllerTests
    {
        private Mock<IConversionService> _conversionServiceMock;

        [Fact]
        public async Task GetConvertAsync_Returns_412AndOriginalPayload_WhenErrors()
        {
            // Arrange
            SetupDefaultMocks();
            var serviceResult = new CurrencyConversion()
            {
                Errors = new string[] { "Error1" },
                From = new AmountInCurrency { Amount = 1, Currency = "CUR1" },
                To = new AmountInCurrency { Amount = 2, Currency = "CUR2" },
            };
            _conversionServiceMock.Setup(m => m.ConvertAsync(It.IsAny<CurrencyConversion>())).ReturnsAsync(serviceResult);

            // Act
            var response = await GetController().GetConvertAsync("foo", "bar", 0m) as ObjectResult;
            var responseValue = response.Value as CurrencyConversion;

            // Assert
            Assert.Equal(412, response.StatusCode);
            Assert.Equal("Error1", responseValue.Errors.Single());
            Assert.Equal(serviceResult.From, responseValue.From);
            Assert.Equal(serviceResult.To, responseValue.To);
        }

        [Fact]
        public async Task GetConvertAsync_Returns_200_WhenNoErrors()
        {
            // Arrange
            SetupDefaultMocks();
            var serviceResult = new CurrencyConversion()
            {
                Errors = new string[0],
                From = new AmountInCurrency { Amount = 1, Currency = "CUR1" },
                To = new AmountInCurrency { Amount = 2, Currency = "CUR2" },
            };
            _conversionServiceMock.Setup(m => m.ConvertAsync(It.IsAny<CurrencyConversion>())).ReturnsAsync(serviceResult);

            // Act
            var response = await GetController().GetConvertAsync("foo", "bar", 0m) as ObjectResult;
            var responseValue = response.Value as CurrencyConversion;

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.False(responseValue.Errors.Any());
            Assert.Equal(serviceResult.From, responseValue.From);
            Assert.Equal(serviceResult.To, responseValue.To);
        }

        private void SetupDefaultMocks()
        {
            _conversionServiceMock = new Mock<IConversionService>();
        }

        private ConvertController GetController()
        {
            return new ConvertController(_conversionServiceMock.Object);
        }
    }
}
