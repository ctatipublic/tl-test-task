using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TL.XR.TLExchangeRatesProvider
{
    public class TlExchangeRateResponse
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        [JsonPropertyName("time_last_updated")]
        public int TimeLastUpdated { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
