using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class ClientSentiments
    {
        [JsonProperty(PropertyName = "clientSentiments")]
        public IList<ClientSentiment> Results { get; set; }
    }
}
