using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    [DebuggerDisplay("{Name}= Status:{Status}, ApiKey: {ApiKey}")]
    public class ClientApplication
    {
        public bool AllowEquities { get; set; }

        public bool AllowQuoteOrders { get; set; }

        public int AllowanceAccountHistoricalData { get; set; }

        public int AllowanceAccountOverall { get; set; }

        public int AllowanceAccountTrading { get; set; }

        public int AllowanceApplicationOverall { get; set; }

        public string ApiKey { get; set; }

        public int ConcurrentSubscriptionsLimit { get; set; }

        public string CreatedDate { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
    }
}
