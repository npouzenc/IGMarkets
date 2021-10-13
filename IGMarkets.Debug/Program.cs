using IGMarkets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IGMarkets.Debug
{
    class Program
    {
        static Trading trading;

        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var identifier = configuration["IG:login"];
            var password = configuration["IG:password"];
            var apiKey = configuration["IG:key"];
                        
            try
            {
                trading = IG.Connect(identifier, password, apiKey, isDemo: true);
                await GetBrentPrices(new DateTime(2021, 09, 01), new DateTime(2021, 09, 30));
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
