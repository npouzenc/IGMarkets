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
            var identifier = configuration["IGMarkets:Identifier"];
            var password = configuration["IGMarkets:Password"];
            var apiKey = configuration["IGMarkets:APIKey"];

            
            try
            {
                using (var ig = IG.Connect(identifier, password, apiKey, isDemo: true))
                {
                    await ig.Logout();
                }
            }
            catch (Exception ex)
            {
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
