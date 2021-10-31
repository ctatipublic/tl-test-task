using System.Linq;
using System.Threading.Tasks;
using TL.XR.Core.Validators;
using TL.XR.Infrastructure.DomainModel;
using Xunit;

namespace TL.XR.Core.Tests.Validators
{
    public class CurrencyConversionValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ReturnsEmptyArray_WhenNoErrors()
        {
            // Arrange
            var currencyConversion = new CurrencyConversion
            {
                From = new AmountInCurrency { Amount = 10, Currency = "CUR1" },
                To = new AmountInCurrency { Currency = "CUR2" },
            };

            // Act
            var result = await new CurrencyConversionValidator().ValidateAsync(currencyConversion);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsNullConversion_WhenCurrencyConversionIsNull()
        {
            // Arrange

            // Act
            var result = await new CurrencyConversionValidator().ValidateAsync(null);

            // Assert
            Assert.Equal(CurrencyConversionValidator.Errors.NullConversion, result.Single());
        }

        [Fact]
        public async Task ValidateAsync_ReturnsNullFrom_WhenFromIsNull()
        {
            // Arrange
            var currencyConversion = new CurrencyConversion
            {
                From = null,
                To = new AmountInCurrency { Currency = "CUR2" },
            };

            // Act
            var result = await new CurrencyConversionValidator().ValidateAsync(currencyConversion);

            // Assert
            Assert.Equal(CurrencyConversionValidator.Errors.NullFrom, result.Single());
        }

        [Fact]
        public async Task ValidateAsync_ReturnsNullFrom_WhenToIsNull()
        {
            // Arrange
            var currencyConversion = new CurrencyConversion
            {
                From = new AmountInCurrency { Amount = 10, Currency = "CUR1" },
                To = null,
            };

            // Act
            var result = await new CurrencyConversionValidator().ValidateAsync(currencyConversion);

            // Assert
            Assert.Equal(CurrencyConversionValidator.Errors.NullTo, result.Single());
        }

        [Fact]
        public async Task ValidateAsync_ReturnsMultipleErrors_WhenFromAndToAreNull()
        {
            // Arrange
            var currencyConversion = new CurrencyConversion
            {
                From = null,
                To = null,
            };

            // Act
            var result = await new CurrencyConversionValidator().ValidateAsync(currencyConversion);

            // Assert
            Assert.Equal(CurrencyConversionValidator.Errors.NullFrom, result.ElementAt(0));
            Assert.Equal(CurrencyConversionValidator.Errors.NullTo, result.ElementAt(1));
        }

    }
}
