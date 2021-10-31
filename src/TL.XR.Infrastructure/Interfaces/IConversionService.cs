using System.Threading.Tasks;
using TL.XR.Infrastructure.DomainModel;

namespace TL.XR.Infrastructure.Interfaces
{
    public interface IConversionService
    {
        /// <summary>
        /// Expects a CurrencyConversion object, with the From and To Currency and From Amount filled in.
        /// </summary>
        /// <param name="currencyConversion"></param>
        /// <returns>A Currency Conversion object with both From And To AmountInCurrency filled in</returns>
        Task<CurrencyConversion> ConvertAsync(CurrencyConversion currencyConversion);
    }
}
