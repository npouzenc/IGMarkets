using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;
using System.Threading.Tasks;
using IGMarkets;
using Flurl.Http.Testing;
using System.Net.Http;

namespace IGMarkets.Tests
{
    [TestFixture]
    public class IGTest
    {
        private HttpTest httpTest;

        [SetUp]
        public void Setup()
        {
            httpTest = new HttpTest();
        }

        [Test]
        public void IG_Login()
        {
            // Arrange
            var loginJsonResponse = new
            {
                clientId = 100557439,
                accountId = "ABCDE",
                timezoneOffset = 1,
                lightstreamerEndpoint = "https://demo-apd.marketdatasystems.com",
                oauthToken = new
                {
                    access_token = "11111111-2222-3333-4444-555555555555",
                    refresh_token = "011111111-2222-3333-4444-555555555555",
                    scope = "profile",
                    token_type = "Bearer",
                    expires_in = 60
                }
            };
            httpTest.RespondWithJson(loginJsonResponse);

            // Act
            var tradingSession = IG.Connect("identifier", "password", "aaaaabbbbbcccccddddeeee", isDemo: true);

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
        public void IG_Logout()
        {
            // Arrange
            var loginJsonResponse = new
            {
                clientId = 100557439,
                accountId = "ABCDE",
                timezoneOffset = 1,
                lightstreamerEndpoint = "https://demo-apd.marketdatasystems.com",
                oauthToken = new
                {
                    access_token = "11111111-2222-3333-4444-555555555555",
                    refresh_token = "011111111-2222-3333-4444-555555555555",
                    scope = "profile",
                    token_type = "Bearer",
                    expires_in = 60
                }
            };
            httpTest.RespondWithJson(loginJsonResponse);
            var tradingSession = IG.Connect("identifier", "password", "aaaaabbbbbcccccddddeeee", isDemo: true);

            // Act
            tradingSession.Logout().Wait();

            // Assert
            Assert.IsFalse(tradingSession.IsConnected);
            httpTest.ShouldHaveCalled("https://demo-api.ig.com/gateway/deal/session")
                .WithVerb(HttpMethod.Delete)
                .WithHeader("VERSION")
                .WithHeader("X-IG-API-KEY");
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            httpTest.Dispose();
        }
    }
}