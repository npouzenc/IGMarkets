using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    internal class Markets
    {
        [JsonProperty(PropertyName = "marketDetails")]
        public IList<Market> Results { get; set; }
    }
}
