using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class AccountInfo
    {
        public double Balance { get; set; }
        public double Deposit { get; set; }
        public double ProfitLoss { get; set; }
        public double Available { get; set; }
    }
}
