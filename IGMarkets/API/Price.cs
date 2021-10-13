using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    [DebuggerDisplay("Time={SnapshotTime}, Close: {Close}")]
    public class Price
    {
        public string SnapshotTime { get; set; }

        public string SnapshotTimeUTC { get; set; }

        public int LastTradedVolume { get; set; }

        [JsonProperty(PropertyName = "openPrice")]
        public PriceData Open { get; set; }

        [JsonProperty(PropertyName = "closePrice")]
        public PriceData Close { get; set; }

        [JsonProperty(PropertyName = "highPrice")]
        public PriceData High { get; set; }

        [JsonProperty(PropertyName = "lowPrice")]
        public PriceData Low { get; set; }
    }
}
