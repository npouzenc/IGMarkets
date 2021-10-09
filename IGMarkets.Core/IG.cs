using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace IGMarkets.Core
{
    public class IG : IDisposable
    {
        private Session session;
        private ILogger<IG> logger;

        public bool IsConnected { get; private set; }

        private string api = "https://api.ig.com";

        public IG(ILogger<IG> logger)
        {
            this.logger = logger;
            session = new Session();
        }

        public async Task Login(string identifier, string password, string apiKey, bool demo = false)
        {
            logger.LogInformation("Trying to connect to IG Markets.");
            if (demo)
            {
                logger.LogInformation("Using DEMO account and DEMO API endpoints.");
                api = "https://demo-api.ig.com";
            }

            try
            {
                Session session = await api.AppendPathSegment("/gateway/deal/session")
                .WithHeader("VERSION", "2")
                .WithHeader("X-IG-API-KEY", apiKey)
                .ConfigureRequest(settings => settings.AfterCallAsync = LogResponse)
                .PostJsonAsync(new { identifier = identifier, password = password })
                .ReceiveJson<Session>();

                IsConnected = true;
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                logger.LogError(ex, $"Error returned from {ex.Call.Request.Url}: {error.SomeDetails}");
            }
        }

        public void Dispose()
        {
            // TODO: Force Logout
        }

        private async Task LogResponse(FlurlCall call)
        {
            var response = await call.Response.GetStringAsync();
            logger.LogDebug(call.ToString() + ": " + response);
        }
    }
}
