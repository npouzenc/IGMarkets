using System.Collections.Generic;

namespace IGMarkets.API
{
    public class PricesResult
    {
        public string InstrumentType { get; set; }

        public Metadata Metadata { get; set; }

        public IList<Price> Prices { get; set; }
    }
}
