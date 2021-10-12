using IGMarkets.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets
{
    public interface ITrading : IDisposable
    {

        public bool IsConnected { get; }

        Session Session{ get; }

        Task RefreshSession();

        Task<IList<SearchMarketResult>> SearchMarkets(string searchTerm);

        Task<IList<MarketDetails>> GetMarkets(bool snapshotOnly = false, params string[] epics);

        Task<MarketDetails> GetMarket(string epic);
    }
}
