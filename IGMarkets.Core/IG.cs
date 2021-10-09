using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using IGMarkets.Core.Resources;

namespace IGMarkets.Core
{
    public class IG : IDisposable
    {
        private Session session;
        private ILogger<IG> logger;
        private Credentials credentials;

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
            this.credentials = new Credentials(identifier, password, apiKey);
            if (demo)
            {
                logger.LogInformation("Using DEMO account and DEMO API endpoints.");
                api = "https://demo-api.ig.com";
            }

            try
            {
                this.session = await api.AppendPathSegment("/gateway/deal/session")
                    .WithHeaders(new { VERSION = 3, X_IG_API_KEY = apiKey })
                    .ConfigureRequest(settings => settings.AfterCallAsync = LogResponse)
                    .PostJsonAsync(new { identifier = identifier, password = password })
                    .ReceiveJson<Session>();

                IsConnected = true;
            }
            catch (FlurlHttpException ex)
            {
                logger.LogError(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
        }

        public async Task Logout()
        {
            try
            {
                await api.AppendPathSegment("/gateway/deal/session")
                    .WithHeader("VERSION", "1")
                    .WithHeader("X-IG-API-KEY", credentials.APIKey)
                    .WithHeader("IG-ACCOUNT-ID", session.AccountId)
                    .WithOAuthBearerToken(session.OAuthToken.AccessToken)
                    .ConfigureRequest(settings => settings.AfterCallAsync = LogResponse)
                    .DeleteAsync();

                IsConnected = false;
            }
            catch (FlurlHttpException ex)
            {
                logger.LogError(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (IsConnected)
            {
                Logout().Wait();
            }
        }

        private async Task LogResponse(FlurlCall call)
        {
            var response = await call.Response.ResponseMessage.Content.ReadAsStringAsync();
            logger.LogDebug(call.ToString() + ": " + response);
        }
    }
}
