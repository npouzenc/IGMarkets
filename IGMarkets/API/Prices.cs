using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class Prices
    {
        public string InstrumentType { get; set; }

        public Metadata Metadata { get; set; }

        [JsonProperty(PropertyName = "prices")]
        public IList<Price> Results { get; set; }
    }
}
