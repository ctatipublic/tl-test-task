using System.Collections.Generic;
using System.Threading.Tasks;

namespace TL.XR.Infrastructure.Interfaces
{
    public interface IExchangeRatesProvider
    {
        Task<Dictionary<string, string>> GetExchangeRatesAsyncForIsoCurrencyAsync(string isoCurrency);
    }
}
