using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    [DebuggerDisplay("{MarketId}= Long:{LongPositionPercentage}, Short: {ShortPositionPercentage}")]
    public class ClientSentiment
    {
        public float LongPositionPercentage { get; set; }

        public float ShortPositionPercentage { get; set; }

        public string MarketId { get; set; }
    }
}
