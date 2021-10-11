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
    }
}
