namespace IGMarkets.API
{
    public class CurrencyData
    {
        ///<Summary>
        /// Code, to be used when placing orders
        ///</Summary>
        public string Code { get; set; }
        ///<Summary>
        /// Symbol, for display purposes
        ///</Summary>
        public string Symbol { get; set; }
        ///<Summary>
        /// Base exchange rate
        ///</Summary>
        public decimal? BaseExchangeRate { get; set; }

        public decimal? ExchangeRate { get; set; }

        ///<Summary>
        /// True if this is the default currency
        ///</Summary>
        public bool IsDefault { get; set; }
    }
}