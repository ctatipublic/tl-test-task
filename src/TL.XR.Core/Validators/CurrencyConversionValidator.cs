using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TL.XR.Infrastructure.DomainModel;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.Core.Validators
{
    public class CurrencyConversionValidator : IValidator<CurrencyConversion>
    {
        public static class Errors
        {
            public const string NullConversion = "NULL_CONVERSION";
            public const string NullFrom = "NULL_FROM";
            public const string NullTo = "NULL_TO";
        }

        public Task<IEnumerable<string>> ValidateAsync(CurrencyConversion value)
        {
            var errors = new List<string>();

            if (value == null)
            {
                errors.Add(Errors.NullConversion);
            }
            else
            {
                if (value.From == null) { errors.Add(Errors.NullFrom); }
                if (value.To == null) { errors.Add(Errors.NullTo); }
            }

            return Task.FromResult(errors.AsEnumerable());
        }
    }
}
