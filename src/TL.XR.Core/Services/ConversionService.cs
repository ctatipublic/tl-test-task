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

        public ConversionService(IEnumerable<IValidator<CurrencyConversion>> currencyConversionValidators)
        {
            _currencyConversionValidators = currencyConversionValidators;
        }

        public async Task<CurrencyConversion> ConvertAsync(CurrencyConversion currencyConversion)
        {
            var errors = new List<string>();
            foreach(var validator in _currencyConversionValidators)
            {
                errors.AddRange(await validator.ValidateAsync(currencyConversion));
            }

            if (!errors.Any())
            {

            }
            return null;
        }
    }
}
