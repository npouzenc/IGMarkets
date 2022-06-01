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
using Polly;
using Polly.Retry;

namespace IGMarkets
{
    /// <summary>
    /// Trading class encapsulating calls on IP Markets Trading API, cf. https://labs.ig.com/rest-trading-api-reference/
    /// </summary>
    public class Trading : IDisposable
    {
        /// <summary>
        /// Credentials for IGMarkets.
        /// </summary>
        private Credentials _credentials;

        /// <summary>
        /// NLog
        /// </summary>
        private Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Current session once connected.
        /// </summary>
        public Session Session { get; private set; }

        /// <summary>
        /// Creates a Polly Policy to retry once if facing an authorization issue (catched with a 401 http response). Refreshing the "access token" if this is the case.
        /// </summary>
        private AsyncRetryPolicy RetryPolicy
        {
            get
            {
                return Policy
                    .Handle<FlurlHttpException>(exception => exception.StatusCode == 401) // Handling HTTP 401: error.security.oauth-token-invalid
                    .RetryAsync(1, onRetry: async (exception, attemptNumber) => await RefreshSession());
            }
        }

        /// <summary>
        /// Creates a new HttpRequest to be used when calling the IG Markets REST API.
        /// </summary>
        private IFlurlRequest IG(string endpoint, int version = 1) => new IGRequest(_credentials, Session).Endpoint(endpoint, version);

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

        #region /session endpoints

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
            await Login(new Credentials(identifier, password, apiKey, isDemo));
        }

        public async Task Login(Credentials credentials) 
        {
            Guard.Against.Null(credentials, nameof(credentials));
            _logger.Debug($"Creating a dealing session with IG Markets for identifier '{credentials.Identifier}'");
            this._credentials = credentials;
            try
            {
                this.Session = await IG("/session", 3)
                    .PostJsonAsync(new { identifier = credentials.Identifier, password = credentials.Password })
                    .ReceiveJson<Session>();

                IsConnected = true;
            }
            catch (FlurlHttpException ex)
            {
                _logger.Fatal(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
                throw; // If we cannot login ...
            }
        }

        public async Task Logout()
        {
            _logger.Debug($"Closing the dealing session on account '{Session.AccountId}' for identifier '{_credentials.Identifier}'");
            try
            {
                await IG("/session").DeleteAsync();
            }
            catch (FlurlHttpException ex)
            {
                _logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
            finally
            {
                IsConnected = false;
            }
        }

        public async Task RefreshSession()
        {
            _logger.Debug($"Refreshing the dealing session on account '{Session.AccountId}' for identifier '{_credentials.Identifier}'");
            try
            {
                var refreshToken = Session.OAuthToken.Refresh_token;
                Session.OAuthToken = null; // Deleting invalid access token to prevent an error from IG when requesting /session endpoint

                Session.OAuthToken = await IG("/session/refresh-token")
                    .PostJsonAsync(new { refresh_token = refreshToken })
                    .ReceiveJson<OAuthToken>();

                IsConnected = true;
            }
            catch (FlurlHttpException ex)
            {
                _logger.Error(ex, $"Error returned from {ex.Call.Request.Url}: {ex.Message}");
            }
        }
        #endregion

        #region /marketnavigation endpoints

        public async Task<IList<MarketNavigationNode>> GetMarketNavigation(string nodeId = "")
        {
            _logger.Debug($"Requesting the market navigation hierarchy (market categories) for node ID [{nodeId}].");

            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/marketnavigation/" + nodeId).GetJsonAsync<MarketNavigationResult>()
            );

            return response.Nodes ?? new List<MarketNavigationNode>();
        }

        #endregion

        #region /markets endpoints

        public async Task<IList<SearchMarketResult>> SearchMarkets(string searchTerm)
        {
            _logger.Debug($"Searching markets with the term '{searchTerm}'");

            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/markets?searchTerm=")
                    .SetQueryParam("searchTerm", searchTerm, true)
                    .GetJsonAsync<SearchMarketsResult>()
            );
            
            return response.Results ?? new List<SearchMarketResult>();   
        }

        /// <summary>
        /// Returns the details of the given markets.
        /// </summary>
        /// <param name="epics">List of markets to retrieve.</param>
        /// <param name="snapshotOnly">If false (default value) then display the market snapshot and minimal instrument data fields. Else display all market details. </param>
        /// <returns>Markets details</returns>
        public async Task<IList<Market>> GetMarkets(bool snapshotOnly = false, params string[] epics)
        {
            Guard.Against.Null(epics, "epics", nameof(epics));
            Guard.Against.OutOfRange(epics.Length, "epics", 1, 50);

            string epicsQueryParam = string.Join(',', epics);

            _logger.Debug($"Looking for the following markets: {epics}");
            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/markets", version: 2)
                    .SetQueryParam("epics", epicsQueryParam, true)
                    .SetQueryParam("filter", snapshotOnly ? "SNAPSHOT_ONLY" : "ALL")
                    .GetJsonAsync<MarketsResult>()
            );

            return response.MarketDetails ?? new List<Market>();
        }

        public async Task<Market> GetMarket(string epic)
        {
            Guard.Against.NullOrEmpty(epic, nameof(epic));

            _logger.Debug($"Looking for the following market: {epic}");

            return await RetryPolicy.ExecuteAsync(
                () => IG("/markets/" + epic, version: 3).GetJsonAsync<Market>()
            );
        }

        #endregion

        #region /prices endpoints

        public async Task<IList<Price>> GetPrices(string epic, Timeframe timeframe, int maxNumberOfPricePoints = 10)
        {
            Guard.Against.NullOrEmpty(epic, nameof(epic));

            _logger.Debug($"Requesting {epic} {maxNumberOfPricePoints} most recent prices (timeframe: {timeframe})");
            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/prices/" + epic, version: 3)
                    .SetQueryParam("resolution", timeframe)
                    .SetQueryParam("max", maxNumberOfPricePoints)
                    .SetQueryParam("pageSize", 0) // disabling paging
                    .GetJsonAsync<PricesResult>() // Care of MaxAllowance of 10,000 points of data per week...
            );
            
            return response.Prices ?? new List<Price>();
        }

        public async Task<IList<Price>> GetPrices(string epic, Timeframe timeframe, DateTime from, DateTime to)
        {
            Guard.Against.NullOrEmpty(epic, nameof(epic));
            Guard.Against.Default<DateTime>(from, nameof(from));
            Guard.Against.Default<DateTime>(to, nameof(to));

            _logger.Debug($"Requesting {epic} prices  between {from.ToUniversalTime()} and {to.ToUniversalTime()} (timeframe: {timeframe})");

            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/prices/" + epic, version: 3)
                    .SetQueryParam("resolution", timeframe)
                    .SetQueryParam("from", from.ToString("s"))
                    .SetQueryParam("to", to.ToString("s"))
                    .SetQueryParam("pageSize", 0) // disabling paging
                    .GetJsonAsync<PricesResult>() // Care of MaxAllowance of 10,000 points of data per week...
            );

            return response.Prices ?? new List<Price>();
        }

        #endregion

        #region /clientsentiment endpoints

        public async Task<IList<ClientSentiment>> GetSentiments(params string[] marketIds)
        {
            Guard.Against.NullOrEmpty(marketIds, nameof(marketIds));

            string markets = string.Join(",", marketIds);

            _logger.Debug($"Requesting client sentiment for the following market: {markets}");

            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/clientsentiment?marketIds=" + markets).GetJsonAsync<ClientSentimentResults>()
            );

            return response.ClientSentiments ?? new List<ClientSentiment>();
        }

        #endregion

        #region /watchlists endpoints

        public async Task<IList<Watchlist>> GetWatchlists()
        {
            _logger.Debug($"Requesting watchlists for the account: {Session.AccountId}");
            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/watchlists").GetJsonAsync<WatchlistsResult>()
            );

            return response.Watchlists ?? new List<Watchlist>();
        }

        public async Task<IList<WatchlistMarket>> GetWatchlist(string id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            _logger.Debug($"Requesting watchlist id:{id}");

            var response = await RetryPolicy.ExecuteAsync(
                () => IG("/watchlists/" + id)
                    .GetJsonAsync<WatchlistMarkets>()
            );
            
            return response.Markets ?? new List<WatchlistMarket>();
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

        private IGRequest CreateRequest()
        {
            return new IGRequest(_credentials, Session);
        }

        private void LogRequest(FlurlCall call)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(ReadSanitizedRequest(call));
            }
        }

        private async Task LogResponse(FlurlCall call)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(await ReadResponse(call));
            }
        }

        private string ReadSanitizedRequest(FlurlCall call)
        {
            string verb = call.Request.Verb.ToString();
            string path = call.Request.Url.Path;
            string body = path == "/gateway/deal/session" ? "*******" : call.RequestBody.JsonPrettify();
            
            return $"--> {verb} {call.Request.Url}: {body}";
        }

        private async Task<string> ReadResponse(FlurlCall call)
        {
            var response = await call.Response.ResponseMessage.Content.ReadAsStringAsync();
            return $"<-- {call}: {response}";
        }

        private bool IsWorthRetrying(FlurlHttpException ex)
        {
            switch (ex.Call.Response.StatusCode)
            {
                case 401:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}
