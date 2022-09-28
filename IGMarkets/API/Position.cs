namespace IGMarkets.API
{
    public class Position
    {
        ///<Summary>
        /// Size of the contract
        ///</Summary>
        public decimal? ContractSize { get; set; }
        ///<Summary>
        /// Date the position was opened
        ///</Summary>
        public string CreatedDate { get; set; }
        ///<Summary>
        /// Deal identifier
        ///</Summary>
        public string DealId { get; set; }
        ///<Summary>
        /// Deal size
        ///</Summary>
        public decimal? DealSize { get; set; }
        ///<Summary>
        /// Deal direction
        ///</Summary>
        public string Direction { get; set; }
        ///<Summary>
        /// Limit level
        ///</Summary>
        public decimal? LimitLevel { get; set; }
        ///<Summary>
        /// Level at which the position was opened
        ///</Summary>
        public decimal? OpenLevel { get; set; }
        ///<Summary>
        /// Position currency ISO code
        ///</Summary>
        public string Currency { get; set; }
        ///<Summary>
        /// True if position is risk controlled
        ///</Summary>
        public bool ControlledRisk { get; set; }
        ///<Summary>
        /// Stop level
        ///</Summary>
        public decimal? StopLevel { get; set; }
        ///<Summary>
        /// Trailing step size
        ///</Summary>
        public decimal? TrailingStep { get; set; }
        ///<Summary>
        /// Trailing stop distance
        ///</Summary>
        public decimal? TrailingStopDistance { get; set; }
    }
}