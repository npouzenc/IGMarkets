using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class Watchlists
    {
        [JsonProperty(PropertyName = "watchlists")]
        ///<Summary>
        /// List of watchlists
        ///</Summary>
        public IList<Watchlist> Results { get; set; }
    }
}
