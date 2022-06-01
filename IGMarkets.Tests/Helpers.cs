using Flurl.Http.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.Tests
{
    public class Helpers
    {
        protected HttpTest _httpTest;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }


        /// <summary>
        /// Load string from embbeded resource
        /// </summary>
        /// <param name="jsonFilename">Json file for instance</param>
        /// <returns>string representation of the file asked</returns>
        protected string LoadResource(string jsonFilename)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("IGMarkets.Tests.Json." + jsonFilename))
            using (var reader = new System.IO.StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result.Trim();
            }
        }

        protected void ArrangeHttpSessionResponse(bool demo, int expiresInSeconds = 60)
        {
            var loginJsonResponse = new
            {
                clientId = 3,
                accountId = "XXXXX",
                timezoneOffset = 1,
                lightstreamerEndpoint = demo ? "https://demo-apd.marketdatasystems.com" : "https://apd.marketdatasystems.com",
                oauthToken = new
                {
                    access_token = Guid.NewGuid(),
                    refresh_token = Guid.NewGuid(),
                    scope = "profile",
                    token_type = "Bearer",
                    expires_in = expiresInSeconds
                }
            };
            _httpTest.RespondWithJson(loginJsonResponse);
        }

        protected Trading Connect()
        {
            ArrangeHttpSessionResponse(demo: true); // New Http response when calling /session
            var trading = IG.Connect("Nicolas", "p@ssw0rd", "zzzzzzzzzzzzzzzzzzzzz", isDemo: true);
            return trading;
        }
    }
}
