using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    [DebuggerDisplay("{Instrument.Name}")]
    public class Market
    {
        ///<Summary>
        /// Instrument data
        ///</Summary>
        public InstrumentData Instrument { get; set; }
        ///<Summary>
        /// Dealing rule data
        ///</Summary>
        public DealingRulesData DealingRules { get; set; }
        ///<Summary>
        /// Market snapshot data
        ///</Summary>
        public MarketSnapshotData Snapshot { get; set; }
    }
}
