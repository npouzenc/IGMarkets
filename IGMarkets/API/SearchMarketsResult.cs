using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    public class SearchMarketsResult
    {
        ///<Summary>
        /// List of markets
        ///</Summary>
        public List<SearchMarketResult> Markets { get; set; }
    }
}
