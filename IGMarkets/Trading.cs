using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using IGMarkets.API;
using NLog;

namespace IGMarkets
{
    public class Trading : ITrading
    {
        private Credentials credentials;
        private Logger logger = LogManager.GetCurrentClassLogger();
        public Session Session { get; private set; }


        public bool IsConnected { get; private set; }

        public Trading()
        {
            Session = new Session();
            FlurlHttp.Configure(settings =>
            {
                settings.BeforeCall = LogRequest;
                settings.AfterCallAsync = LogResponse;
            });
        }

        #region /session REST API endpoints

        /// <summary>
        /// Asynchronous login to created a new trading session on IGMarkets with specified credentials.
        /// </summary>
        /// <param name="identifier">Username of the account to connect to.</param>
        /// <param name="password">Password to use</param>
        /// <param name="apiKey">The API key k (obtained from My Account on our dealing platform) is how we identify and authorise the calling application</param>
        /// <param name="isDemo">Are you using a LIVE account or a DEMO account?</param>
        /// <returns></returns>
        public async Task Login(string identifier, string password, string apiKey, bool isDemo = false)
        {
            logger.Info($"Creating a dealing session with IG Markets for identifier '{identifier}'");
            this.credentials = new Credentials(identifier, password, apiKey, isDemo);

            try
            {
                var request = new IGRequest(credentials, Session);
                this.Session = await request.Create("/session", 3)
                    .PostJsonAsync(new { identifier = identifier, password = password })
                    .ReceiveJson<Session>();

                IsConnected = true;
            }
            catch (FlurlHttpException ex)
            {
                logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
        }

        public async Task Logout()
        {
            logger.Info($"Closing the dealing session on account '{Session.AccountId}' for identifier '{credentials.Identifier}'");
            try
            {
                var request = new IGRequest(credentials, Session);
                await request.Create("/session")
                    .DeleteAsync();

                IsConnected = false;
            }
            catch (FlurlHttpException ex)
            {
                logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
        }
        #endregion



        public async void Dispose()
        {
            if (IsConnected)
            {
                await Logout();
            }
        }

        private void LogRequest(FlurlCall call)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug($"--> {call.Request.Verb} {call.Request.Url}: {call.RequestBody}");
            }
        }

        private async Task LogResponse(FlurlCall call)
        {
            if (logger.IsDebugEnabled)
            {
                var response = await call.Response.ResponseMessage.Content.ReadAsStringAsync();
                logger.Debug($"<-- {call}: {response}");
            }
        }
    }
}
