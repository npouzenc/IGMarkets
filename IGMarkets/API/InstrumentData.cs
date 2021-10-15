using System.Collections.Generic;
using System.Diagnostics;

namespace IGMarkets.API
{
    [DebuggerDisplay("{Type}= Epic:{Epic}, MarketId={MarketId}")]
    public class InstrumentData
    {
        ///<Summary>
        /// Instrument identifier
        ///</Summary>
        public string Epic { get; set; }
        ///<Summary>
        /// Expiry
        ///</Summary>
        public string Expiry { get; set; }
        ///<Summary>
        /// Name
        ///</Summary>
        public string Name { get; set; }
        ///<Summary>
        /// True if force open is allowed
        ///</Summary>
        public bool ForceOpenAllowed { get; set; }
        ///<Summary>
        /// True if stops and limits are allowed
        ///</Summary>
        public bool StopsLimitsAllowed { get; set; }
        ///<Summary>
        /// Lot size
        ///</Summary>
        public decimal? LotSize { get; set; }
        ///<Summary>
        /// Unit
        ///</Summary>
        public string Unit { get; set; }
        ///<Summary>
        /// Type
        ///</Summary>
        public string Type { get; set; }
        ///<Summary>
        /// True if controlled risk trades are allowed
        ///</Summary>
        public bool ControlledRiskAllowed { get; set; }
        ///<Summary>
        /// True if streaming prices are available, i.e. the market is open and the client has appropriate permissions
        ///</Summary>
        public bool StreamingPricesAvailable { get; set; }
        ///<Summary>
        /// Market identifier
        ///</Summary>
        public string MarketId { get; set; }
        ///<Summary>
        /// Available currencies
        ///</Summary>
        public List<CurrencyData> Currencies { get; set; }
        ///<Summary>
        /// For sprint markets only, the minimum value to be specified as the expiry of a sprint markets trade
        ///</Summary>
        public int? SprintMarketsMinimumExpiryTime { get; set; }
        ///<Summary>
        /// For sprint markets only, the maximum value to be specified as the expiry of a sprint markets trade
        ///</Summary>
        public int? SprintMarketsMaximumExpiryTime { get; set; }
        ///<Summary>
        /// Margin deposit requirement bands
        ///</Summary>
        public List<DepositBand> MarginDepositBands { get; set; }
        ///<Summary>
        /// Margin requirement factor
        ///</Summary>
        public decimal? MarginFactor { get; set; }
        ///<Summary>
        /// Size unit for the margin requirements
        ///</Summary>
        public string MarginFactorUnit { get; set; }
        ///<Summary>
        /// Slippage factor
        ///</Summary>
        public SlippageFactorData SlippageFactor { get; set; }
        ///<Summary>
        /// Opening hours
        ///</Summary>
        public OpeningHours OpeningHours { get; set; }
        ///<Summary>
        /// Expiry details
        ///</Summary>
        public MarketExpiryData ExpiryDetails { get; set; }
        ///<Summary>
        /// Rollover details
        ///</Summary>
        public MarketRolloverData RolloverDetails { get; set; }
        ///<Summary>
        /// Reuters news code
        ///</Summary>
        public string NewsCode { get; set; }
        ///<Summary>
        /// Chart code
        ///</Summary>
        public string ChartCode { get; set; }
        ///<Summary>
        /// Country
        ///</Summary>
        public string Country { get; set; }
        ///<Summary>
        /// Value of one pip
        ///</Summary>
        public string ValueOfOnePip { get; set; }
        ///<Summary>
        /// Meaning of one pip
        ///</Summary>
        public string OnePipMeans { get; set; }
        ///<Summary>
        /// Contract size
        ///</Summary>
        public string ContractSize { get; set; }
        ///<Summary>
        /// List of special information notices
        ///</Summary>
        public List<string> SpecialInfo { get; set; }
    }
}