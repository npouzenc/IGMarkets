using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class WatchlistMarket
    {
        public string InstrumentName { get; set; }

        public string InstrumentType { get; set; }

        public string Epic { get; set; }

        ///<Summary>
        /// Expiry
        ///</Summary>
        public string Expiry { get; set; }

        ///<Summary>
        /// Highest price on the day
        ///</Summary>
        public decimal? High { get; set; }

        ///<Summary>
        /// Lowest price on the day
        ///</Summary>
        public decimal? Low { get; set; }

        ///<Summary>
        /// Bid price
        ///</Summary>
        public decimal? Bid { get; set; }

        ///<Summary>
        /// Offer price
        ///</Summary>
        public decimal? Offer { get; set; }

        public string MarketStatus { get; set; }

        ///<Summary>
        /// Multiplying factor to determine actual pip value for the levels used by the instrument
        ///</Summary>
        public int ScalingFactor { get; set; }

        ///<Summary>
        /// Lot size
        ///</Summary>
        public decimal? LotSize { get; set; }

        ///<Summary>
        /// True if streaming prices are available, i.e. the market is open and the client has appropriate permissions
        ///</Summary>
        public bool StreamingPricesAvailable { get; set; }

        ///<Summary>
        /// Price last update time (hh:mm:ss)
        ///</Summary>
        public string UpdateTime { get; set; }


        ///<Summary>
        /// Price last update time (hh:mm:ss) in UTC
        ///</Summary>
        public string UpdateTimeUTC { get; set; }

        ///<Summary>
        /// Price delay
        ///</Summary>
        public int DelayTime { get; set; }
    }
}
