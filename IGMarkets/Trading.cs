using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Flurl;
using Flurl.Http;
using IGMarkets.API;
using NLog;

namespace IGMarkets
{
    public class Trading : ITrading
    {
        /// <summary>
        /// Credentials for IGMarkets.
        /// </summary>
        private Credentials credentials;

        /// <summary>
        /// NLog
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Current session once connected.
        /// </summary>
        public Session Session { get; private set; }

        /// <summary>
        /// Is the current session connected to IGMarkets ?
        /// </summary>
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
                this.Session = await request
                    .Endpoint("/session", 3)
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
                await request
                    .Endpoint("/session")
                    .DeleteAsync();

                IsConnected = false;
            }
            catch (FlurlHttpException ex)
            {
                logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
        }

        public async Task RefreshSession()
        {
            logger.Info($"Refreshing the dealing session on account '{Session.AccountId}' for identifier '{credentials.Identifier}'");
            try
            {
                var request = new IGRequest(credentials, Session);

                Session = await request
                    .Endpoint("/session/refresh-token")
                    .PostJsonAsync(new { refresh_token = Session.OAuthToken.RefreshToken })
                    .ReceiveJson<Session>();

                IsConnected = true;
            }
            catch (FlurlHttpException ex)
            {
                logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
        }
        #endregion

        #region /markets REST API endpoints

        public async Task<IList<SearchMarketResult>> SearchMarkets(string searchTerm)
        {
            logger.Info($"Searching markets with the term '{searchTerm}'");
            try
            {
                var request = new IGRequest(credentials, Session);

                var searchResults = await request
                    .Endpoint("/markets?searchTerm=")
                    .SetQueryParam("searchTerm", searchTerm, true)
                    .GetJsonAsync<SearchMarketsResult>();
                return searchResults.Markets;   
            }
            catch (FlurlHttpException ex)
            {
                logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// Returns the details of the given markets.
        /// </summary>
        /// <param name="epics">List of markets to retrieve.</param>
        /// <param name="snapshotOnly">If false (default value) then display the market snapshot and minimal instrument data fields. Else display all market details. </param>
        /// <returns>Markets details</returns>
        public async Task<IList<MarketDetails>> GetMarkets(bool snapshotOnly = false, params string[] epics)
        {
            Guard.Against.Null(epics, "epics", nameof(epics));
            Guard.Against.OutOfRange(epics.Length, "epics", 1, 50);

            string epicsQueryParam = string.Join(',', epics);

            logger.Info($"Looking for the following markets: {epics}");
            try
            {
                var request = new IGRequest(credentials, Session);

                var response = await request
                    .Endpoint("/markets", version: 2)
                    .SetQueryParam("epics", epicsQueryParam, true)
                    .SetQueryParam("filter", snapshotOnly ? "SNAPSHOT_ONLY":"ALL")
                    .GetJsonAsync<MarketsDetails>();

                return response.MarketDetails;
                
            }
            catch (FlurlHttpException ex)
            {
                logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
                throw;
            }

        }

        public async Task<MarketDetails> GetMarket(string epic)
        {
            Guard.Against.NullOrEmpty(epic, nameof(epic));

            logger.Info($"Looking for the following market: {epic}");
            try
            {
                var request = new IGRequest(credentials, Session);

                var market = await request
                    .Endpoint("/markets/" + epic, version: 3)
                    .GetJsonAsync<MarketDetails>();

                return market;

            }
            catch (FlurlHttpException ex)
            {
                logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region IDisposable

        public async void Dispose()
        {
            if (IsConnected)
            {
                await Logout();
            }
        }
        #endregion

        #region Private methods

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

        #endregion
    }
}
