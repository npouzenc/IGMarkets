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
        private Credentials credentials;
        private ILogger<IG> logger;
        private Session session;
        
        public bool IsConnected { get; private set; }

        private string api = "https://api.ig.com/gateway/deal";

        public IG(ILogger<IG> logger)
        {
            this.logger = logger;
            session = new Session();
        }

        public async Task Login(string identifier, string password, string apiKey, bool demo = false)
        {
            logger.LogInformation($"Creating a dealing session with IG Markets for identifier {identifier}");
            this.credentials = new Credentials(identifier, password, apiKey);
            if (demo)
            {
                api = api.Replace("api", "demo-api");
            }

            try
            {
                this.session = await api.AppendPathSegment("/session")
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
            logger.LogInformation($"Closing the dealing session {session.AccountId} for identifier {credentials.Identifier}");
            try
            {
                await (api + "/session")
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

        public async void Dispose()
        {
            if (IsConnected)
            {
                await Logout();
            }
        }

        private async Task LogResponse(FlurlCall call)
        {
            var response = await call.Response.ResponseMessage.Content.ReadAsStringAsync();
            logger.LogDebug(call.ToString() + ": " + response);
        }
    }
}
