using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Testing;
using NUnit.Framework;

namespace IGMarkets.Tests
{
    [TestFixture]
    public class IGRequestTest : Helpers
    {
        [Test]
        public async Task IGRequest_RefreshingTokenIfObsolete()
        {
            // Arrange
            ArrangeHttpSessionResponse(demo: true, expiresInSeconds: 0); // getting a valid OAuth token that immediatly expires
            var trading = IG.Connect("Nicolas", "p@ssw0rd", "zzzzzzzzzzzzzzzzzzzzz", isDemo: true);
            _httpTest.RespondWithJson(new { errorCode = "error.security.oauth-token-invalid" }, 401); // simulating a invalid token response from IG

            // Act & Assert
            // Call failed with status code 401 (Unauthorized): GET https://demo-api.ig.com/gateway/deal/marketnavigation/
            Assert.ThrowsAsync<FlurlHttpException>(async () => await trading.GetMarketNavigation());
        }
    }
}
