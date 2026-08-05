using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LBank.Net.Interfaces.Clients.SpotApi;
using LBank.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using LBank.Net.Clients.MessageHandlers;
using LBank.Net.Objects.Internal;

namespace LBank.Net.Clients.SpotApi
{
    /// <inheritdoc cref="ILBankRestClientSpotApi" />
    internal partial class LBankRestClientSpotApi : RestApiClient<LBankEnvironment, LBankAuthenticationProvider, LBankCredentials>, ILBankRestClientSpotApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => LBankErrors.Errors;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new LBankRestMessageHandler(LBankErrors.Errors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public ILBankRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public ILBankRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public ILBankRestClientSpotApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "LBank";
        #endregion

        #region constructor/destructor
        internal LBankRestClientSpotApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, LBankRestOptions options)
            : base(loggerFactory, LBankExchange.Metadata.Id, httpClient, options.Environment.RestClientSpotAddress, options, options.SpotOptions)
        {
            RequestBodyFormat = RequestBodyFormat.FormData;

            Account = new LBankRestClientSpotApiAccount(this);
            ExchangeData = new LBankRestClientSpotApiExchangeData(_logger, this);
            Trading = new LBankRestClientSpotApiTrading(_logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(LBankExchange._serializerContext);


        /// <inheritdoc />
        protected override LBankAuthenticationProvider CreateAuthenticationProvider(LBankCredentials credentials)
            => new LBankAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<LBankResponse>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail(result);

            if (!result.Data.Result)
                return HttpResult.Fail(result, new ServerError(result.Data.ErrorCode, GetErrorInfo(result.Data.ErrorCode, result.Data.Message)));

            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<LBankResponse<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            if (!result.Data.Result)
                return HttpResult.Fail<T>(result, new ServerError(result.Data.ErrorCode, GetErrorInfo(result.Data.ErrorCode, result.Data.Message)));

            return HttpResult.Ok(result, result.Data.Data!);
        }

        internal async Task<HttpResult<T>> SendRawAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => LBankExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public ILBankRestClientSpotApiShared SharedClient => this;
    }
}
