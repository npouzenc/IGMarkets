using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace IGMarkets.API
{
    public class WatchlistMarkets
    {
        ///<Summary>
        /// List of watchlists
        ///</Summary>
        public IList<WatchlistMarket> Markets { get; set; }
    }
}
