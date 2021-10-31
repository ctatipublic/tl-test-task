using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TL.XR.Infrastructure.DomainModel;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.Core.Services
{
    public class ConversionService : IConversionService
    {
        private readonly IEnumerable<IValidator<CurrencyConversion>> _currencyConversionValidators;
        private readonly IExchangeRatesProvider _exchangeRatesProvider;

        public static class Errors
        {
            public const string UnableToConvertError = "UNABLE_TO_CONVERT";
        }

        public ConversionService(IEnumerable<IValidator<CurrencyConversion>> currencyConversionValidators, IExchangeRatesProvider exchangeRatesProvider)
        {
            _currencyConversionValidators = currencyConversionValidators;
            _exchangeRatesProvider = exchangeRatesProvider;
        }

        public async Task<CurrencyConversion> ConvertAsync(CurrencyConversion currencyConversion)
        {
            var errors = new List<string>();
            var result = new CurrencyConversion() { Errors = errors, From = currencyConversion.From, To = currencyConversion.To };

            foreach (var validator in _currencyConversionValidators)
            {
                errors.AddRange(await validator.ValidateAsync(currencyConversion));
            }

            if (!errors.Any())
            {
                var exchangeRates = await _exchangeRatesProvider.GetExchangeRatesAsyncForIsoCurrencyAsync(currencyConversion.From.Currency);
                var exchangeRate = exchangeRates?.SingleOrDefault(r => r.Key.Equals(currencyConversion.To.Currency, StringComparison.OrdinalIgnoreCase)).Value;
                if (!exchangeRate.HasValue || exchangeRate == 0m)
                {
                    errors.Add(Errors.UnableToConvertError);
                }
                else
                {
                    currencyConversion.To.Amount = currencyConversion.From.Amount * exchangeRate.Value;
                }
            }
            return result;
        }
    }
}
