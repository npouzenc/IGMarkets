namespace IGMarkets.API
{
    public class DealingRulesData
    {
        ///<Summary>
        /// Minimum step distance
        ///</Summary>
        public DealingRuleData MinStepDistance { get; set; }
        ///<Summary>
        /// Minimum deal size
        ///</Summary>
        public DealingRuleData MinDealSize { get; set; }
        ///<Summary>
        /// Minimum controlled risk stop distance
        ///</Summary>
        public DealingRuleData MinControlledRiskStopDistance { get; set; }
        ///<Summary>
        /// Minimum stop or limit distance
        ///</Summary>
        public DealingRuleData MinNormalStopOrLimitDistance { get; set; }
        ///<Summary>
        /// Maximum stop or limit distance
        ///</Summary>
        public DealingRuleData MaxStopOrLimitDistance { get; set; }
        ///<Summary>
        /// The client's market order preference for creating or closing positions.
        /// This should be ignored when editing positions and when creating, editing and deleting working orders.
        ///</Summary>
        public string MarketOrderPreference { get; set; }
        ///<Summary>
        /// Determines whether the user is allowed to set trailing stops for this particular market
        ///</Summary>
        public string TrailingStopsPreference { get; set; }
    }
}