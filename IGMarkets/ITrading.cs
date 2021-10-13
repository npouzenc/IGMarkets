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

        Task<IList<Market>> GetMarkets(bool snapshotOnly = false, params string[] epics);

        Task<Market> GetMarket(string epic);

        Task<IList<Price>> GetPrices(string epic);
    }
}
