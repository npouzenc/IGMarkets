namespace IGMarkets.API
{
    public class DepositBand
    {
        ///<Summary>
        /// Band minimum
        ///</Summary>
        public float? Min { get; set; }
        ///<Summary>
        /// Band maximum
        ///</Summary>
        public float? Max { get; set; }
        ///<Summary>
        /// Margin Percentage
        ///</Summary>
        public decimal? Margin { get; set; }
        ///<Summary>
        /// The currency for this currency band factor calculation
        ///</Summary>
        public string Currency { get; set; }
    }
}