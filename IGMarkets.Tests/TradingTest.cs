using Flurl.Http.Testing;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IGMarkets.Tests
{
    [TestFixture]
    public class TradingTest : Helpers
    {
        [SetUp]
        public void Setup()
        {
            httpTest = new HttpTest();
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            httpTest.Dispose();
        }

        #region Tests for /session
        [Test]
        public void Session_Login()
        {
            // Arrange
            var loginJsonResponse = new
            {
                clientId = 16180339,
                accountId = "XXXXX",
                timezoneOffset = 1,
                lightstreamerEndpoint = "https://demo-apd.marketdatasystems.com",
                oauthToken = new
                {
                    access_token = Guid.NewGuid().ToString(),
                    refresh_token = Guid.NewGuid().ToString(),
                    scope = "profile",
                    token_type = "Bearer",
                    expires_in = 60
                }
            };
            httpTest.RespondWithJson(loginJsonResponse);

            // Act
            var tradingSession = IG.Connect("Nicolas", "p@ssw0rd", "zzzzzzzzzzzzzzzzzzzzz", isDemo: true);

            // Assert
            Assert.IsTrue(tradingSession.IsConnected);
            Assert.AreEqual(loginJsonResponse.accountId, tradingSession.Session.AccountId);
            Assert.AreEqual(loginJsonResponse.clientId.ToString(), tradingSession.Session.ClientId);
            Assert.AreEqual(new System.Uri(loginJsonResponse.lightstreamerEndpoint), tradingSession.Session.LightstreamerEndpoint);
            Assert.AreEqual(loginJsonResponse.timezoneOffset, tradingSession.Session.TimezoneOffset);
            Assert.IsNotNull(tradingSession.Session.OAuthToken);
            Assert.AreEqual(loginJsonResponse.oauthToken.access_token, tradingSession.Session.OAuthToken.AccessToken);
            Assert.AreEqual(loginJsonResponse.oauthToken.refresh_token, tradingSession.Session.OAuthToken.RefreshToken);
            Assert.AreEqual(loginJsonResponse.oauthToken.expires_in, tradingSession.Session.OAuthToken.ExpiresIn);
            Assert.AreEqual(loginJsonResponse.oauthToken.scope, tradingSession.Session.OAuthToken.Scope);
            Assert.AreEqual(loginJsonResponse.oauthToken.token_type, tradingSession.Session.OAuthToken.TokenType);
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/session")
                .WithVerb(HttpMethod.Post)
                .WithHeader("VERSION")
                .WithHeader("X-IG-API-KEY");
        }

        [Test]
        public void Session_Logout()
        {
            // Arrange
            ITrading trading = Connect();

            // Act
            trading.Dispose();

            // Assert
            Assert.IsFalse(trading.IsConnected);
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/session")
                .WithVerb(HttpMethod.Delete)
                .WithHeader("VERSION")
                .WithHeader("X-IG-API-KEY");
        }

        [Test]
        public void Session_Refresh()
        {
            // Arrange
            ITrading trading = Connect();
            string refreshToken = trading.Session.OAuthToken.RefreshToken;
            ArrangeHttpSessionResponse(demo: true); // New Http response when calling /session/refresh-token

            // Act
            trading.RefreshSession();

            // Assert
            Assert.IsTrue(trading.IsConnected);
            Assert.AreNotEqual(refreshToken, trading.Session.OAuthToken.RefreshToken);
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/session/refresh-token")
                .WithVerb(HttpMethod.Post)
                .WithHeader("VERSION")
                .WithHeader("X-IG-API-KEY")
                .WithOAuthBearerToken()
                .WithRequestBody("*refresh_token*");
        }

        #endregion

        #region Tests for /markets

        [Test]
        public void Markets_GetMarkets_OutOfRange()
        {
            // Arrange
            ITrading trading = Connect();
            string[] epics = new string[51];

            // Act && Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await trading.GetMarkets(snapshotOnly: false, epics));
        }

        [Test]
        // https://labs.ig.com/rest-trading-api-reference/service-detail?id=590
        public async Task Markets_GetMarkets_AllDetails()
        {
            // Arrange
            ITrading trading = Connect();
            var jsonResponse = LoadResource("markets_CS.D.EURUSD.CFD.IP+CS.D.EURUSD.MINI.IP.json");
            httpTest.RespondWith(jsonResponse);

            // Act
            var marketsDetails = await trading.GetMarkets(snapshotOnly: false, 
                "CS.D.EURUSD.CFD.IP", "CS.D.EURUSD.MINI.IP");

            // Assert
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/markets?epics=CS.D.EURUSD.CFD.IP,CS.D.EURUSD.MINI.IP")
                .WithVerb(HttpMethod.Get)
                .WithHeader("VERSION")
                .WithHeader("X-IG-API-KEY")
                .WithOAuthBearerToken();
            Assert.AreEqual(2, marketsDetails.Count);
            var cfdEURUSD = marketsDetails[0];
            var miniEURUSD = marketsDetails[1];
            Assert.IsNotNull(cfdEURUSD.DealingRules);
            Assert.IsNotNull(cfdEURUSD.Instrument);
            Assert.IsNotNull(cfdEURUSD.Snapshot);
            Assert.AreEqual("CS.D.EURUSD.CFD.IP", cfdEURUSD.Instrument.Epic);
            Assert.AreEqual("CS.D.EURUSD.MINI.IP", miniEURUSD.Instrument.Epic);
            Assert.AreEqual("EURUSD", cfdEURUSD.Instrument.MarketId);
            Assert.IsTrue(cfdEURUSD.Instrument.ForceOpenAllowed);
            Assert.IsTrue(cfdEURUSD.Instrument.StopsLimitsAllowed);
            Assert.IsTrue(cfdEURUSD.Instrument.ControlledRiskAllowed);
            Assert.IsTrue(cfdEURUSD.Instrument.StreamingPricesAvailable);
            Assert.IsNull(cfdEURUSD.Instrument.SprintMarketsMaximumExpiryTime);
            Assert.IsNull(cfdEURUSD.Instrument.SprintMarketsMinimumExpiryTime);
            Assert.AreEqual(1.1559, cfdEURUSD.Instrument.Currencies[0].BaseExchangeRate);
            Assert.AreEqual(0.66, cfdEURUSD.Instrument.Currencies[0].ExchangeRate);
            // Really IG? You need THAT precision for margin factor?!
            Assert.AreEqual(3.330000000000000071054273576m, cfdEURUSD.Instrument.MarginFactor);
            Assert.AreEqual("AVAILABLE_DEFAULT_OFF", miniEURUSD.DealingRules.MarketOrderPreference);
            Assert.AreEqual("AVAILABLE", miniEURUSD.DealingRules.TrailingStopsPreference);
            Assert.AreEqual(1.15587, miniEURUSD.Snapshot.Bid);
            Assert.AreEqual(1.15593, miniEURUSD.Snapshot.Offer);
            Assert.AreEqual("10:06:44", miniEURUSD.Snapshot.UpdateTime);
            Assert.AreEqual(5.0, miniEURUSD.DealingRules.MinStepDistance.Value);
        }

        [Test]
        public async Task Markets_GetMarkets_SnapshotOnly()
        {
            // Arrange
            ITrading trading = Connect();
            var jsonResponse = LoadResource("markets_CS.D.EURUSD.CFD.IP (snapshot only).json");
            httpTest.RespondWith(jsonResponse);

            // Act
            var marketsDetails = await trading.GetMarkets(snapshotOnly: true, "CS.D.EURUSD.CFD.IP");

            // Assert
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/markets?epics=CS.D.EURUSD.CFD.IP&filter=SNAPSHOT_ONLY")
                .WithVerb(HttpMethod.Get)
                .WithHeader("VERSION")
                .WithHeader("X-IG-API-KEY")
                .WithOAuthBearerToken();
            Assert.AreEqual(1, marketsDetails.Count);
            var cfdEURUSD = marketsDetails[0];
            Assert.IsNull(cfdEURUSD.DealingRules);
            Assert.IsNotNull(cfdEURUSD.Instrument);
            Assert.IsNotNull(cfdEURUSD.Snapshot);
            Assert.AreEqual("CS.D.EURUSD.CFD.IP", cfdEURUSD.Instrument.Epic);
            Assert.AreEqual("CURRENCIES", cfdEURUSD.Instrument.Type);
            Assert.AreEqual(1.15424, cfdEURUSD.Snapshot.Bid);
            Assert.AreEqual(1.15433, cfdEURUSD.Snapshot.Offer);
            Assert.AreEqual(1.15708, cfdEURUSD.Snapshot.High);
            Assert.AreEqual(1.15329, cfdEURUSD.Snapshot.Low);
            Assert.AreEqual("14:01:11", cfdEURUSD.Snapshot.UpdateTime);
            Assert.AreEqual(-0.08, cfdEURUSD.Snapshot.PercentageChange);
            Assert.AreEqual(-0.00096, cfdEURUSD.Snapshot.NetChange);
        }

        [Test]
        public async Task Markets_GetMarket()
        {
            // Arrange
            ITrading trading = Connect();
            var jsonResponse = LoadResource("markets_CS.D.EURUSD.MINI.IP.json");
            httpTest.RespondWith(jsonResponse);

            // Act
            var miniEURUSD = await trading.GetMarket("CS.D.EURUSD.MINI.IP");

            // Assert
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/markets/CS.D.EURUSD.MINI.IP")
                .WithVerb(HttpMethod.Get)
                .WithHeader("VERSION")
                .WithHeader("X-IG-API-KEY")
                .WithOAuthBearerToken();
            Assert.IsNotNull(miniEURUSD);
            Assert.IsNotNull(miniEURUSD.Instrument);
            Assert.IsNotNull(miniEURUSD.Snapshot);
            Assert.AreEqual("CS.D.EURUSD.MINI.IP", miniEURUSD.Instrument.Epic);
            Assert.AreEqual("CURRENCIES", miniEURUSD.Instrument.Type);
            Assert.AreEqual("10000", miniEURUSD.Instrument.ContractSize);
            Assert.AreEqual(1.15402, miniEURUSD.Snapshot.Bid);
            Assert.AreEqual(1.15411, miniEURUSD.Snapshot.Offer);
            Assert.AreEqual(1.15708, miniEURUSD.Snapshot.High);
            Assert.AreEqual(1.15329, miniEURUSD.Snapshot.Low);
            Assert.AreEqual("16:04:17", miniEURUSD.Snapshot.UpdateTime);
            Assert.AreEqual("TRADEABLE", miniEURUSD.Snapshot.MarketStatus);
        }
        #endregion

        #region Tests for /prices

        [TestCase("CS.D.EURUSD.MINI.IP", "2021/10/13 09:06:00", 1.15476f, 1.15485f)]
        [TestCase("CC.D.LCO.UNC.IP", "2021/10/13 09:13:00", 8290.2f, 8293.0f)]
        public async Task Prices_GetPrices(string instrument, string snapshotTime, float firstOpenPriceBid, float firstOpenPriceAsk)
        {
            // Arrange
            ITrading trading = Connect();
            var jsonFile = $"prices_{instrument}.json";
            httpTest.RespondWith(LoadResource(jsonFile));

            // Act
            var prices = await trading.GetPrices(instrument);

            // Assert
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/prices/" + instrument)
                .WithVerb(HttpMethod.Get)
                .WithHeader("VERSION", 3)
                .WithHeader("X-IG-API-KEY")
                .WithOAuthBearerToken();
            Assert.IsNotEmpty(prices);
            Assert.AreEqual(10, prices.Count);
            Assert.AreEqual(snapshotTime, prices[0].SnapshotTime);
            Assert.AreEqual(firstOpenPriceBid, prices[0].Open.Bid);
            Assert.AreEqual(firstOpenPriceAsk, prices[0].Open.Ask);
        }

        #endregion
    }
}