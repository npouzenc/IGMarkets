using System.Collections.Generic;

namespace IGMarkets.API
{
    public class OpeningHours
    {
        ///<Summary>
        /// List of market open and close times (in the account timezone)
        ///</Summary>
        public List<TimeRange> MarketTimes { get; set; }
    }
}