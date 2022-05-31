using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    [DebuggerDisplay("Time={SnapshotTime}, Close: {ClosePrice}")]
    public class Price
    {
        public string SnapshotTime { get; set; }

        public string SnapshotTimeUTC { get; set; }

        public int LastTradedVolume { get; set; }

        public PriceData OpenPrice { get; set; }

        public PriceData ClosePrice { get; set; }

        public PriceData HighPrice { get; set; }

        public PriceData LowPrice { get; set; }
    }
}
