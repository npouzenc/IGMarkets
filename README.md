# Unofficial IG Markets Trading API for C# or dotnet apps

Unofficial IG Markets Trading API for C# or dotnet based applications. IGMarkets is a modern, fluent, asynchronous and portable library for .NET applications to consume IGMarkets API.

Disclaimer: This library is **not associated** with IG Markets Limited or any of its affiliates or subsidiaries. If you use this library, it's at your own risk.

## Usage

```csharp
using IGMarkets;

using var trading = IG.Connect(login, password, apiKey);

string epic = "CC.D.LCO.UNC.IP"; // Brent
var prices = await trading.GetPrices(epic, Timeframe.DAY);

foreach (var price in prices)
{
    Console.WriteLine($"\t{price.SnapshotTime}: O:[{price.OpenPrice}] C:[{price.ClosePrice}] H:[{price.HighPrice}] L:[{price.LowPrice}]");
}
```

## Endpoints progress

Account:

- [x] /accounts
- [ ] /accounts/preferences
- [ ] /history
- [ ] /history/activity
- [ ] /history/activity/fromDate/toDate
- [ ] /history/activity/lastPeriod
- [ ] /history/transactions
- [ ] /history/transactions/transactionType

Dealing:

- [ ] /confirms/dealReference
- [ ] /positions
- [ ] /positions/dealId
- [ ] /positions/otc
- [ ] /positions/otc/dealId
- [ ] /positions/sprintmarkets
- [ ] /workingorders
- [ ] /workingorders/otc
- [ ] /workingorders/otc/dealId

General:

- [x] /operations/application
- [ ] /operations/application/disable

Markets:

- [x] /marketsnavigation
- [x] /markets
- [x] /markets/epic
- [x] /markets?searchTerm=
- [x] /prices
- [x] /prices/epic

Watchlists:

- [x] /watchlists
- [x] /watchlists/id

Client Sentiment:

- [x] /clientsentiment
- [x] /clientsentiment/marketId
- [ ] /clientsentiment/related/marketId

Session:

- [x] /session
- [x] /session/refresh-token