using System;
using IGMarkets;
using Microsoft.Extensions.Configuration;

var config = ConfigurationBuilder();
var login = config["IG:login"];
var password = config["IG:password"];
var apiKey = config["IG:apiKey"];

using var trading = IG.Connect(login, password, apiKey, isDemo: true);

string epic = "CC.D.LCO.UNC.IP";
var prices = await trading.GetPrices(epic, Timeframe.DAY);

foreach (var price in prices)
{
    Console.WriteLine($"\t{price.SnapshotTime}: O:[{price.OpenPrice}] C:[{price.ClosePrice}] H:[{price.HighPrice}] L:[{price.LowPrice}]");
}

static IConfigurationRoot ConfigurationBuilder()
{
    return new ConfigurationBuilder()
        .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
        .AddUserSecrets<Program>()
        .Build();
}

static async System.Threading.Tasks.Task Navigate(Trading trading)
{
    var navigation = await trading.GetMarketNavigation();

    Console.WriteLine(navigation);

    foreach (var node in navigation)
    {
        Console.WriteLine($"{node.ID} ({node.Name}):");
        var subNavigation = await trading.GetMarketNavigation(node.ID);
        foreach (var subNode in subNavigation)
        {
            Console.WriteLine($"\t{subNode.ID} ({subNode.Name})");
        }
        System.Threading.Thread.Sleep(10000); // avoiding error.public-api.exceeded-api-key-allowance
    }
}

/*    
 *    Some examples:
 *

static async System.Threading.Tasks.Task TestRefreshRoken(Trading trading)
{
    Console.WriteLine($"access_token: {trading.Session.OAuthToken.Access_token}");
    Console.WriteLine($"expires at: {trading.Session.OAuthToken.GetExpirationDate().ToShortTimeString()}");
    Console.WriteLine($"refresh_token: {trading.Session.OAuthToken.Refresh_token}");

    Console.WriteLine();

    await trading.RefreshSession();
    Console.WriteLine($"access_token: {trading.Session.OAuthToken.Access_token}");
    Console.WriteLine($"expires at: {trading.Session.OAuthToken.GetExpirationDate().ToLongTimeString()}");
    Console.WriteLine($"refresh_token: {trading.Session.OAuthToken.Refresh_token}");
}
 
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
*/