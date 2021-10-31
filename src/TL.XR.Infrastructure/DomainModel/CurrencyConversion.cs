using System.Collections.Generic;

namespace TL.XR.Infrastructure.DomainModel
{
    public class CurrencyConversion
    {
        public AmountInCurrency From { get; set; }
        public AmountInCurrency To { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}
