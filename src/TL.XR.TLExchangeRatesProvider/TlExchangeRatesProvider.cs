using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.TLExchangeRatesProvider
{
    public class TlExchangeRatesProvider : IExchangeRatesProvider
    {
        public Task<Dictionary<string, string>> GetExchangeRatesAsyncForIsoCurrencyAsync(string isoCurrency)
        {
            throw new NotImplementedException();
        }
    }
}
