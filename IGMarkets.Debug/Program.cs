using IGMarkets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IGMarkets.Debug
{
    class Program
    {
        static Trading trading;

        const string BRENT = "CC.D.LCO.UNC.IP";
        const string CAC40 = "IX.D.CAC.IMF.IP";
        const string DAX40 = "IX.D.DAX.IDF.IP";
        const string DOW30 = "IX.D.DOW.IDF.IP";
        const string EURUSD = "CS.D.EURUSD.CFD.IP";
        const string FTSE = "IX.D.FTSE.IFE.IP";

        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var identifier = configuration["IG:login"];
            var password = configuration["IG:password"];
            var apiKey = configuration["IG:apiKey"];
                        
            try
            {
                trading = IG.Connect(identifier, password, apiKey, isDemo: true);
                //await GetBrentPrices(new DateTime(2021, 09, 01), new DateTime(2021, 09, 30));
                //await GetSentiments();
                await GetWatchlists();
            }
            catch (Exception ex)
            {
                /// Last chance exception handling
                Console.Error.WriteLine("Critical error when sending request to IG Markets REST Trading API:");
                Console.Error.WriteLine(ex);
            }
            finally
            {
                trading.Dispose();
            }
        }

        private static async Task GetWatchlists()
        {
            var watchlists = await trading.GetWatchlists();
            foreach (var watchlist in watchlists)
            {
                Console.WriteLine($"\t{watchlist.Id}: {watchlist.Name} (editable? {watchlist.Editable} - deletable? {watchlist.Deleteable}");
            }
            if (watchlists.Count > 0)
            {
                var watchlist = watchlists.First();
                var markets = await trading.GetWatchlist(watchlist.Id);
                Console.WriteLine($"\t{watchlist.Id}: {watchlist.Name} contains the following markets:");
                foreach (var market in markets)
                {
                    Console.WriteLine($"\t\tMarket: {market.Instrument.Name} ({market.Instrument.Epic})");
                }
            }
            
        }

        private static async Task GetSentiments()
        {
            var market = await trading.GetMarket(DAX40);
            var sentiments = await trading.GetSentiments("FR40", "DE30", "EURUSD");
            foreach (var sentiment in sentiments)
            {
                Console.WriteLine($"\t{sentiment.MarketId}: [LONG:{sentiment.LongPositionPercentage} SHORT:{sentiment.ShortPositionPercentage}]");
            }            
        }

        private static async Task GetLastBrentDailyPrices()
        {
            string epic = "CC.D.LCO.UNC.IP";
            var prices = await trading.GetPrices(epic, Timeframe.DAY);
            Console.WriteLine($"{epic}: {prices.Count} results");
            foreach (var price in prices)
            {
                Console.WriteLine($"\t{price.SnapshotTime}: O:[{price.Open}] C:[{price.Close}] H:[{price.High}] L:[{price.Low}]");
            }
        }

        private static async Task GetBrentPrices(DateTime from, DateTime to)
        {
            string epic = "CC.D.LCO.UME.IP";
            var prices = await trading.GetPrices(epic, Timeframe.DAY, from, to);
            Console.WriteLine($"{epic}: {prices.Count} results");
            foreach (var price in prices)
            {
                Console.WriteLine($"\t{price.SnapshotTime}: O:[{price.Open}] C:[{price.Close}] H:[{price.High}] L:[{price.Low}]");
            }
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddUserSecrets<Program>()
                .Build();
        }
    }
}
