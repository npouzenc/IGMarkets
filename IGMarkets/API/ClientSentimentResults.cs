﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class ClientSentimentResults
    {
        [JsonPropertyName("clientSentiments")]
        public IList<ClientSentiment> ClientSentiments { get; set; }
    }
}
