namespace IGMarkets.API
{
    public class SearchMarketResult
    {
        ///<Summary>
        /// Instrument epic identifier
        ///</Summary>
        public string Epic { get; set; }
        ///<Summary>
        /// Instrument name
        ///</Summary>
        public string InstrumentName { get; set; }
        ///<Summary>
        /// Instrument type
        ///</Summary>
        public string InstrumentType { get; set; }
        ///<Summary>
        /// Instrument expiry period
        ///</Summary>
        public string Expiry { get; set; }
        ///<Summary>
        /// Highest price of the day
        ///</Summary>
        public decimal? High { get; set; }
        ///<Summary>
        /// Lowest price of the day
        ///</Summary>
        public decimal? Low { get; set; }
        ///<Summary>
        /// Percentage price change on the day
        ///</Summary>
        public decimal? PercentageChange { get; set; }
        ///<Summary>
        /// Price net change
        ///</Summary>
        public decimal? NetChange { get; set; }
        ///<Summary>
        /// Time of last price update
        ///</Summary>
        public string UpdateTime { get; set; }
        ///<Summary>
        /// Bid price
        ///</Summary>
        public decimal? Bid { get; set; }
        ///<Summary>
        /// Offer price
        ///</Summary>
        public decimal? Offer { get; set; }
        ///<Summary>
        /// Price delay time in minutes
        ///</Summary>
        public int DelayTime { get; set; }
        ///<Summary>
        /// True if streaming prices are available, i.e. if the market is tradeable and the client has appropriate permissions
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