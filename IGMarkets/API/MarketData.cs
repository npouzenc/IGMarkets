using System.Diagnostics;
using System;

namespace IGMarkets.API
{
    [DebuggerDisplay("Epic = {Epic}, MarketStatus = {MarketStatus}, UpdateTime = {UpdateTime}")]
    public class MarketData
    {
        ///<Summary>
        /// Instrument name
        ///</Summary>
        public string InstrumentName { get; set; }
        ///<Summary>
        /// Instrument expiry period
        ///</Summary>
        public string Expiry { get; set; }
        ///<Summary>
        /// Instrument epic identifier
        ///</Summary>
        public string Epic { get; set; }
        ///<Summary>
        /// Instrument type
        ///</Summary>
        public string InstrumentType { get; set; }
        ///<Summary>
        /// Instrument lot size
        ///</Summary>
        public decimal? LotSize { get; set; }
        ///<Summary>
        /// High price
        ///</Summary>
        public decimal? High { get; set; }
        ///<Summary>
        /// Low price
        ///</Summary>
        public decimal? Low { get; set; }
        ///<Summary>
        /// Price percentage change
        ///</Summary>
        public decimal? PercentageChange { get; set; }
        ///<Summary>
        /// Price net change
        ///</Summary>
        public decimal? NetChange { get; set; }
        ///<Summary>
        /// Bid
        ///</Summary>
        public decimal? Bid { get; set; }
        ///<Summary>
        /// Offer
        ///</Summary>
        public decimal? Offer { get; set; }
        ///<Summary>
        /// Last instrument price update time (HH:mm in universal time)
        ///</Summary>
        public string UpdateTime { get; set; }

        private DateTime? updateDateTime;

        public DateTime? UpdateDateTime
        {
            get
            {
                if (updateDateTime == null)
                {
                    updateDateTime = DateTime.Parse(UpdateTime).ToLocalTime();
                }
                return updateDateTime;
            }
        }

        ///<Summary>
        /// Instrument price delay (minutes)
        ///</Summary>
        public int DelayTime { get; set; }
        ///<Summary>
        /// True if streaming prices are available, i.e. the market is tradeable and the client has appropriate permissions
        ///</Summary>
        public bool StreamingPricesAvailable { get; set; }
        ///<Summary>
        /// Market status
        ///</Summary>
        public string MarketStatus { get; set; }
        ///<Summary>
        /// Multiplying factor to determine actual pip value for the levels used by the instrument
        ///</Summary>
        public int ScalingFactor { get; set; }
    }
}