using System.Threading.Tasks;
using TL.XR.Infrastructure.DomainModel;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.Core.Services
{
    public class ConversionService : IConversionService
    {
        public Task<CurrencyConversion> ConvertAsync(CurrencyConversion currencyConversion)
        {
            throw new System.NotImplementedException();
        }
    }
}
