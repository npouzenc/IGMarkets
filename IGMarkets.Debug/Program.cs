using IGMarkets;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

var configuration = BuildConfiguration();
var identifier = configuration["IG:login"];
var password = configuration["IG:password"];
var apiKey = configuration["IG:apiKey"];
     
using Trading trading = IG.Connect(identifier, password, apiKey, isDemo: true);
await GetBrentPrices(trading, new DateTime(2021, 09, 01), new DateTime(2021, 09, 30));
await GetLastBrentDailyPrices(trading);
await GetSentiments(trading);
await GetWatchlists(trading);
   
        
static async Task GetWatchlists(Trading trading)
{
    var watchlists = await trading.GetWatchlists();
    foreach (var watchlist in watchlists)
    {
        Console.WriteLine($"\t{watchlist.Id}: {watchlist.Name} (editable? {watchlist.Editable} - deletable? {watchlist.Deleteable}");
        var markets = await trading.GetWatchlist(watchlist.Id);
        Console.WriteLine($"\t{watchlist.Id}: {watchlist.Name} contains the following markets:");
        foreach (var market in markets)
        {
            Console.WriteLine($"\t\tMarket: {market.InstrumentName} ({market.Epic})");
        }
    }
            
}

static async Task GetSentiments(Trading trading)
{
    const string DAX40 = "IX.D.DAX.IDF.IP";
    var market = await trading.GetMarket(DAX40);
    var sentiments = await trading.GetSentiments("FR40", "DE30", "EURUSD");
    foreach (var sentiment in sentiments)
    {
        Console.WriteLine($"\t{sentiment.MarketId}: [LONG:{sentiment.LongPositionPercentage} SHORT:{sentiment.ShortPositionPercentage}]");
    }            
}

static async Task GetLastBrentDailyPrices(Trading trading)
{
    string epic = "CC.D.LCO.UNC.IP";
    var prices = await trading.GetPrices(epic, Timeframe.DAY);
    Console.WriteLine($"{epic}: {prices.Count} results");
    foreach (var price in prices)
    {
        Console.WriteLine($"\t{price.SnapshotTime}: O:[{price.Open}] C:[{price.Close}] H:[{price.High}] L:[{price.Low}]");
    }
}

static async Task GetBrentPrices(Trading trading, DateTime from, DateTime to)
{
    string epic = "CC.D.LCO.UME.IP";
    var prices = await trading.GetPrices(epic, Timeframe.DAY, from, to);
    Console.WriteLine($"{epic}: {prices.Count} results");
    foreach (var price in prices)
    {
        Console.WriteLine($"\t{price.SnapshotTime}: O:[{price.Open}] C:[{price.Close}] H:[{price.High}] L:[{price.Low}]");
    }
}

static IConfigurationRoot BuildConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddUserSecrets<Program>()
        .Build();
}
