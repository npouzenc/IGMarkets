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

        public IG(ILogger<IG> logger)
        {
            this.logger = logger;
            session = new Session();
            FlurlHttp.Configure(settings =>
            {
                settings.BeforeCall = LogRequest;
                settings.AfterCallAsync = LogResponse;
            });
        }

        public async Task Login(string identifier, string password, string apiKey, bool isDemo = false)
        {
            logger.LogInformation($"Creating a dealing session with IG Markets for identifier '{identifier}'");
            this.credentials = new Credentials(identifier, password, apiKey, isDemo);
            
            try
            {
                var request = new IGRequest(credentials, session);
                this.session = await request.Create("/session", 3)
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
            logger.LogInformation($"Closing the dealing session on account '{session.AccountId}' for identifier '{credentials.Identifier}'");
            try
            {
                var request = new IGRequest(credentials, session);
                await request.Create("/session")
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

        private void LogRequest(FlurlCall call)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"--> {call.Request.Verb} {call.Request.Url}: {call.RequestBody}");
            }
        }

        private async Task LogResponse(FlurlCall call)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var response = await call.Response.ResponseMessage.Content.ReadAsStringAsync();
                logger.LogDebug($"<-- {call}: {response}");
            }
        }
    }
}
