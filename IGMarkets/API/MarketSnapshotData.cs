namespace IGMarkets.API
{
    public class MarketSnapshotData
    {
        ///<Summary>
        /// Market status
        ///</Summary>
        public string MarketStatus { get; set; }
        ///<Summary>
        /// Net price change on the day
        ///</Summary>
        public decimal? NetChange { get; set; }
        ///<Summary>
        /// Percentage price change on the day
        ///</Summary>
        public decimal? PercentageChange { get; set; }
        ///<Summary>
        /// Price last update time (hh:mm:ss)
        ///</Summary>
        public string UpdateTime { get; set; }
        ///<Summary>
        /// Price delay
        ///</Summary>
        public int DelayTime { get; set; }
        ///<Summary>
        /// Bid price
        ///</Summary>
        public decimal? Bid { get; set; }
        ///<Summary>
        /// Offer price
        ///</Summary>
        public decimal? Offer { get; set; }
        ///<Summary>
        /// Highest price on the day
        ///</Summary>
        public decimal? High { get; set; }
        ///<Summary>
        /// Lowest price on the day
        ///</Summary>
        public decimal? Low { get; set; }
        ///<Summary>
        /// Binary odds
        ///</Summary>
        public decimal? BinaryOdds { get; set; }
        ///<Summary>
        /// Number of decimal positions for market levels
        ///</Summary>
        public int DecimalPlacesFactor { get; set; }
        ///<Summary>
        /// Multiplying factor to determine actual pip value for the levels used by the instrument
        ///</Summary>
        public int ScalingFactor { get; set; }
        ///<Summary>
        /// The number of points to add on each side of the market as an additional spread when
        /// Placing a guaranteed stop trade.
        ///</Summary>
        public decimal? ControlledRiskExtraSpread { get; set; }
    }
}