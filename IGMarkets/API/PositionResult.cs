using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class PositionResult
    {
        public Position Position { get; set; }
        ///<Summary>
        /// Market data
        ///</Summary>
        public MarketData MarketData { get; set; }
    }
}
