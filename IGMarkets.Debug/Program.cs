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
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var identifier = configuration["IG:login"];
            var password = configuration["IG:password"];
            var apiKey = configuration["IG:key"];
                        
            try
            {
                using (var trading = IG.Connect(identifier, password, apiKey, isDemo: true))
                {
                    var results = await trading.GetMarkets(snapshotOnly: true, "CS.D.EURUSD.CFD.IP");
                    Console.WriteLine("Search results:");
                    Console.WriteLine(results.Count);
                }
            }
            catch (Exception ex)
            {
                /// Last chance exception handling
                Console.Error.WriteLine("Critical error when sending request to IG Markets REST Trading API:");
                Console.Error.WriteLine(ex);
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
