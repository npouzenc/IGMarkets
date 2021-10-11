using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;
using System.Threading.Tasks;
using IGMarkets;
using Flurl.Http.Testing;
using System.Net.Http;
using System;

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
            ArrangeHttpSessionResponse(demo: true);
            var trading = IG.Connect("Nicolas", "p@ssw0rd", "zzzzzzzzzzzzzzzzzzzzz", isDemo: true);

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
            ArrangeHttpSessionResponse(demo: true); // New Http response when calling /session
            var trading = IG.Connect("Nicolas", "p@ssw0rd", "zzzzzzzzzzzzzzzzzzzzz", isDemo: true);
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

        public void Trading_MarketNavigation()
        {
            // Arrange
            ArrangeHttpSessionResponse(demo: true); // New Http response when calling /session
            var trading = IG.Connect("Nicolas", "p@ssw0rd", "zzzzzzzzzzzzzzzzzzzzz", isDemo: true);

            // Act
           var markets = trading.SearchMarkets("CAC40");

            // Assert
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

        #endregion
    }
}