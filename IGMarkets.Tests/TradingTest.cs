using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;
using System.Threading.Tasks;
using IGMarkets;
using Flurl.Http;
using Flurl.Http.Testing;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using Flurl.Http.Configuration;

namespace IGMarkets.Tests
{
    [TestFixture]
    public class TradingTest
    {
        private HttpTest httpTest;

        [SetUp]
        public void Setup()
        {
            httpTest = new HttpTest();
        }

        private ITrading Connect()
        {
            ArrangeHttpSessionResponse(demo: true); // New Http response when calling /session
            var trading = IG.Connect("Nicolas", "p@ssw0rd", "zzzzzzzzzzzzzzzzzzzzz", isDemo: true);
            return trading;
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            httpTest.Dispose();
        }

        #region Tests for /session
        [Test]
        public void Trading_Login()
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
        public void Trading_Logout()
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
        public void Trading_Refresh()
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
                .WithRequestBody("*refresh_token*");
        }

        #endregion

        #region Tests for /markets

        [Test]
        public async Task Trading_GetMarkets()
        {
            // Arrange
            ITrading trading = Connect();
            var jsonResponse = LoadResource("markets_CS.D.EURUSD.CFD.IP+CS.D.EURUSD.MINI.IP.json");
            httpTest.RespondWith(jsonResponse);

            // Act
            var marketsDetails = await trading.GetMarkets("CS.D.EURUSD.CFD.IP", "CS.D.EURUSD.MINI.IP");

            // Assert
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
        #endregion

        #region Helper methods

        private void ArrangeHttpSessionResponse(bool demo)
        {
            var loginJsonResponse = new
            {
                clientId = 3,
                accountId = "XXXXX",
                timezoneOffset = 1,
                lightstreamerEndpoint = demo ? "https://demo-apd.marketdatasystems.com": "https://apd.marketdatasystems.com",
                oauthToken = new
                {
                    access_token = Guid.NewGuid(),
                    refresh_token = Guid.NewGuid(),
                    scope = "profile",
                    token_type = "Bearer",
                    expires_in = 60
                }
            };
            httpTest.RespondWithJson(loginJsonResponse);
        }


        private string LoadResource(string filename)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("IGMarkets.Tests.Json." + filename))
            using (var reader = new System.IO.StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result.Trim().Replace("\r\n", string.Empty).Replace("\t", string.Empty);
            }
        }

        #endregion
    }
}